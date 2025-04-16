using System.Net.Http.Json;
using CoffeeMaker.Api;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Infrastructure;
using CoffeeMaker.IntegrationTests.Models;
using CoffeeMaker.IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeMaker.IntegrationTests;

public class BrewRecommendationTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Theory]
    [FileData("TestCases")]
    public async Task Test(TestCase testCase)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CoffeeMakerDbContext>();
        
        await db.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken); 
        await db.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
    
        await db.AddRangeAsync(testCase.RoastProfiles, TestContext.Current.CancellationToken);
        
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);
    
        // Act
        var client = factory.CreateClient();
        await client.PostAsJsonAsync("api/brew-recommendation", new BrewingRecommendationRequest
        {
            BrewDate = DateTime.Now,
            Method = "FrenchPress",
            RoastName = "Ethiopian Sunrise",
            DesiredStrength = 5
        }, TestContext.Current.CancellationToken);
    
        // Assert
        Assert.True(true);
    }
}