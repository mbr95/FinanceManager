using FinanceManager.API.Data;
using FinanceManager.API.Domain.Models;
using FinanceManager.API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public IdentityService(UserManager<IdentityUser> userManager, JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters, DataContext context)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        public async Task<AuthenticationResult> RegisterUserAsync(IdentityUser user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return GetAuthenticationResultWithErrors("User with this email address already exists.");
            }

            var createdUser = await _userManager.CreateAsync(user, user.PasswordHash);

            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.Select(e => e.Description).ToString();
                return GetAuthenticationResultWithErrors(errors);
            }

            return await GenerateAuthenticationResultWithTokenAsync(user);
        }

        public async Task<AuthenticationResult> LoginUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return GetAuthenticationResultWithErrors("User doesn't exist.");
            }
            var userPasswordIsValid = await _userManager.CheckPasswordAsync(user, password);

            if (!userPasswordIsValid)
            {
                return GetAuthenticationResultWithErrors("Invalid password.");
            }

            return await GenerateAuthenticationResultWithTokenAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, Guid refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
                return GetAuthenticationResultWithErrors("Invalid Token");

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix)
                .Subtract(_jwtOptions.TokenLifetime);

            if (expiryDateTime > DateTime.UtcNow)
                return GetAuthenticationResultWithErrors("This token hasn't expired yet.");

            var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshedToken = await _context.RefreshTokens.SingleOrDefaultAsync(e => e.Token == refreshToken);

            if (storedRefreshedToken == null)
                return GetAuthenticationResultWithErrors("This refresh token does not exist.");

            if (DateTime.UtcNow > storedRefreshedToken.ExpiryDate)
                return GetAuthenticationResultWithErrors("This refresh token has expired.");

            if (storedRefreshedToken.Invalidated)
                return GetAuthenticationResultWithErrors("This refresh token has been invalidated.");

            if (storedRefreshedToken.Used)
                return GetAuthenticationResultWithErrors("This refresh token has been used.");

            if (storedRefreshedToken.JwtId != jti)
                return GetAuthenticationResultWithErrors("This refresh token does not match this Jwt.");

            storedRefreshedToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshedToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(c => c.Type == "id").Value);

            return await GenerateAuthenticationResultWithTokenAsync(user);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultWithTokenAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(_jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {

                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token.ToString()
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var claimsPrincipal);
                if (!IsTokenWithValidSecurityAlgorithm(claimsPrincipal))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsTokenWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private AuthenticationResult GetAuthenticationResultWithErrors(string errors)
        {
            return new AuthenticationResult { Errors = new[] { errors } };
        }
    }
}
