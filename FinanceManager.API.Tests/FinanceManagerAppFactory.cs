using FinanceManager.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FinanceManager.API.IntegrationTests
{
    public class FinanceManagerAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var databaseToRemove = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));
                services.Remove(databaseToRemove);

                services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDb"));

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var testDb = scopedServices.GetRequiredService<DataContext>();

                    testDb.Database.EnsureCreated();
                }
            });
        }
    }
}
