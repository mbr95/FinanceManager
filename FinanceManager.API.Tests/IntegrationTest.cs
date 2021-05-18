using FinanceManager.API.Data;
using FinanceManager.API.Requests.v1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinanceManager.API.Responses.v1;
using System.Text;
using System.Linq;
using FinanceManager.API.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FinanceManager.API.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
            var applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<DataContext>));
                        services.Remove(descriptor);
                        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDb"));
                    });
                });
            _serviceProvider = applicationFactory.Services;
            TestClient = applicationFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var registerUserRequest = new RegisterUserRequest
            {
                UserName = "Testuser",
                Email = "testmail@mail.com",
                Password = "Testpassword123!"
            };

            var response = await GetHttpResponse("api/v1/identity/register", registerUserRequest);
            var registerUserResponse = await DeserializeResponse<AuthenticationSucceededResponse>(response);

            return registerUserResponse.Token;
        }

        private async Task<HttpResponseMessage> GetHttpResponse(string requestUri,object request)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(requestUri, stringContent);

            return response;
        }

        protected async Task<TResponseType> DeserializeResponse<TResponseType>(HttpResponseMessage serializedResponse)
        {
            var deserializedObject = JsonConvert.DeserializeObject<TResponseType>(await serializedResponse.Content.ReadAsStringAsync());

            return deserializedObject;
        }
        
        protected async Task InsertCategoriesIntoDatabase(IEnumerable<TransactionCategory> categories)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
            foreach (var category in categories)
            {
                await dataContext.TransactionCategories.AddAsync(category);
                await dataContext.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
            dataContext.Database.EnsureDeleted();
        }
    }
}
