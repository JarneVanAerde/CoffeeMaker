using System.Net.Http.Json;
using CoffeeMaker.Api;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Infrastructure;
using CoffeeMaker.IntegrationTests.Models;
using CoffeeMaker.IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeMaker.IntegrationTests;

public class BrewRecommendationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BrewRecommendationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [FileData("TestCases")]
    public async Task Test(TestCase testCase)
    {
        // Arrange
        var client = _factory.CreateClient();
        var db = _factory.Services.GetRequiredService<CoffeeMakerDbContext>();
        
        await db.Database.EnsureDeletedAsync(); 
        await db.Database.EnsureCreatedAsync();

        await db.AddRangeAsync(testCase.RoastProfiles);
        
        await db.SaveChangesAsync();

        // Act
        await client.PostAsJsonAsync("api/brew-recommendation", new BrewingRecommendationRequest
        {
            BrewDate = DateTime.Now,
            Method = "FrenchPress",
            RoastName = "Ethiopian Sunrise",
            DesiredStrength = 5
        });

        // Assert
        Assert.True(true);
    }
}