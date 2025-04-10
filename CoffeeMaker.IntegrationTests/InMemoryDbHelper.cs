using CoffeeMaker.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.IntegrationTests;

public static class InMemoryDbHelper
{
    public static DbContextOptions<CoffeeMakerDbContext> GetDbOptions()
    {
        return new DbContextOptionsBuilder<CoffeeMakerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), options =>
            {
                options.EnableNullChecks(false);
            })
            .Options;
    }
}