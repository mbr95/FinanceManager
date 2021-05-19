using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Data
{
    public static class IdentityRolesInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<DataContext>();
            await dbContext.Database.MigrateAsync();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userRole = new IdentityRole("StandardUser");
            var adminRole = new IdentityRole("Administrator");

            if(!await roleManager.RoleExistsAsync(userRole.Name))
            {
                await roleManager.CreateAsync(userRole);
            }

            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
