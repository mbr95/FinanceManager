using FinanceManager.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterUserAsync(IdentityUser user);
    }
}
