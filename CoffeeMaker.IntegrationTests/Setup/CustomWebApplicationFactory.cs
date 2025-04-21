using CoffeeMaker.Api;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMaker.IntegrationTests.Setup;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Overwrite the real database implementation with an in-memory one.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CoffeeMakerDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<CoffeeMakerDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb", b => b.EnableNullChecks(false));
                options.EnableSensitiveDataLogging();
            });
        });
    }
}