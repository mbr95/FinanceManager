using FinanceManager.Domain.Models;
using FinanceManager.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(UserManager<IdentityUser> userManager, JwtOptions jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
        }

        public async Task<AuthenticationResult> RegisterUserAsync(IdentityUser user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this email address already exists." } };
            }

            var createdUser = await _userManager.CreateAsync(user, user.PasswordHash);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult { Errors = createdUser.Errors.Select(e => e.Description) };
            }

            return GenerateAuthenticationResultWithToken(user);
        }     

        public async Task<AuthenticationResult> LoginUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return new AuthenticationResult { Errors = new[] { "User doesn't exist." } };                
            }
            var userPasswordIsValid = await _userManager.CheckPasswordAsync(user, password);

            if (!userPasswordIsValid)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid password." } };
            }

            return GenerateAuthenticationResultWithToken(user);
        }

        private AuthenticationResult GenerateAuthenticationResultWithToken(IdentityUser user)
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
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
