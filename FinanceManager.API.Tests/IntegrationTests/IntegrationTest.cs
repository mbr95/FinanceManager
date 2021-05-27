using FinanceManager.API.Data;
using FinanceManager.API.Domain.Models;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Tests.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly FinanceManagerAppFactory<Startup> FinanceManagerAppFactory;
        protected readonly HttpClient TestClient;
        protected readonly IdentityUser TestAdmin = new IdentityUser { UserName = "AdminUser", Email = "administrator1@example.com"};
        protected readonly IdentityUser TestStandardUser = new IdentityUser { UserName = "Testuser", Email = "testmail12@example.com"};
        protected readonly string testAdminPassword = "Newadminpassword123!";
        protected readonly string testUserPassword = "Testpassword123!";

        protected IntegrationTest()
        {
            FinanceManagerAppFactory = new FinanceManagerAppFactory<Startup>();
            TestClient = FinanceManagerAppFactory.CreateClient();
        }

        public void Dispose()
        {
            using(var scope = FinanceManagerAppFactory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var database = serviceProvider.GetRequiredService<DataContext>();
                database.Database.EnsureDeleted();
            }
        }

        protected async Task AuthenticateAsync(IdentityUser userToAuthenticate)
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(userToAuthenticate));
        }

        protected async Task CreateUserDataAsync()
        {
            using(var scope = FinanceManagerAppFactory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<DataContext>();

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var userRole = new IdentityRole("StandardUser");
                var adminRole = new IdentityRole("Administrator");
                
                await roleManager.CreateAsync(userRole);
                await userManager.CreateAsync(TestStandardUser, testUserPassword);
                await userManager.AddToRoleAsync(TestStandardUser, "StandardUser");
                await roleManager.CreateAsync(adminRole);
                await userManager.CreateAsync(TestAdmin, testAdminPassword);
                await userManager.AddToRoleAsync(TestAdmin, "Administrator");                                                
            }            
        }

        protected async Task AddDataToTransactionsDatabaseAsync(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var createTransactionRequest = new CreateTransactionRequest { Description = transaction.Description, Amount = transaction.Amount, Date = transaction.Date, CategoryId = (int)transaction.CategoryId };
                var requestData = new StringContent(JsonConvert.SerializeObject(createTransactionRequest), Encoding.UTF8, "application/json");
                await TestClient.PostAsync("api/v1/transactions", requestData);
            }
        }

        private async Task<string> GetJwtAsync(IdentityUser user)
        {
            var userLoginRequest = new LoginUserRequest
            {
                UserName = user.UserName,
                Password = user.UserName == TestAdmin.UserName ? testAdminPassword : testUserPassword
            };

            var requestData = new StringContent(JsonConvert.SerializeObject(userLoginRequest), Encoding.UTF8, "application/json");
            var loginResponse = await TestClient.PostAsync("api/v1/auth/login", requestData);

            var responseBody = await loginResponse.Content.ReadAsAsync<AuthenticationSucceededResponse>();
            var jwt = responseBody.Token;

            return jwt;
        }
        
    }
}
