using FinanceManager.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterUserAsync(IdentityUser user);
        Task<AuthenticationResult> LoginUserAsync(string userName, string password);
    }
}
