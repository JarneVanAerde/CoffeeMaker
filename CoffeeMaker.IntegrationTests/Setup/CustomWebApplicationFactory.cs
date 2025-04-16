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
            services.Remove(services.Single(d => d.ImplementationType == typeof(CoffeeMakerDbContext)));
            services.AddDbContext<CoffeeMakerDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb", b => b.EnableNullChecks(false));
                options.EnableSensitiveDataLogging();
            });
        });
    }
}