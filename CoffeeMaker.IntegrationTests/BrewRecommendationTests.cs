using System.Net.Http.Json;
using System.Text.Json;
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
        var response = await client.PostAsJsonAsync("api/brew-recommendation", testCase.Request, TestContext.Current.CancellationToken);
    
        //Assert
        BrewingRecommendationResponse? responseContent;
        if (response.IsSuccessStatusCode)
        {
            responseContent = await response.Content.ReadFromJsonAsync<BrewingRecommendationResponse>(TestContext.Current.CancellationToken);
            Assert.Equivalent(testCase.Response, responseContent);      
        }
        else
            throw new Exception(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));

        // Upload attachments
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        TestContext.Current.AddAttachment("actual-response.json", JsonSerializer.Serialize(responseContent, serializerOptions));

        foreach (var attachment in testCase.Attachments)
        {
            TestContext.Current.AddAttachment(attachment.Key, attachment.Value);
        }
    }
}