using FinanceManager.API.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace FinanceManager.API.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> RegisterUserAsync(IdentityUser user);
        Task<AuthenticationResult> LoginUserAsync(string userName, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, Guid refreshToken);
    }
}
