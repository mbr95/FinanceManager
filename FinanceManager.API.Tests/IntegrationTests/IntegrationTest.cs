using FinanceManager.API.Data;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.Tests.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly FinanceManagerAppFactory<Startup> FinanceManagerAppFactory;
        protected readonly HttpClient TestClient;

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

        protected async Task AuthenticateAsync()
        {
            await CreateUserRolesAsync();
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var newUser = new RegisterUserRequest
            {
                UserName = "Testuser",
                Email = "testmail12@example.com",
                Password = "Testpassword123!"
            };

            var requestData = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            var registerResponse = await TestClient.PostAsync("api/v1/auth/register", requestData);

            var responseBody = await registerResponse.Content.ReadAsAsync<AuthenticationSucceededResponse>();
            var jwt = responseBody.Token;

            return jwt;
        }

        private async Task CreateUserRolesAsync()
        {
            using(var scope = FinanceManagerAppFactory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<DataContext>();

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userRole = new IdentityRole("StandardUser");
                var adminRole = new IdentityRole("Administrator");

                await roleManager.CreateAsync(userRole);
                await roleManager.CreateAsync(adminRole);
            }            
        }
    }
}
