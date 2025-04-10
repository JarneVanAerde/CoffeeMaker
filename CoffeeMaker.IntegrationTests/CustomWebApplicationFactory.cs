using CoffeeMaker.Api;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMaker.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
        {
            // Overwrite real database implementation with in-memory variant
            services.Remove(services.Single(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<CoffeeMakerDbContext>)));
            services.AddDbContext<CoffeeMakerDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb", b => b.EnableNullChecks(false));
                options.EnableSensitiveDataLogging();
            });
        });
    }
}