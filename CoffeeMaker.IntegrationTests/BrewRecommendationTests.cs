using CoffeeMaker.Api;
using Xunit;

namespace CoffeeMaker.IntegrationTests;

public class BrewRecommendationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BrewRecommendationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Test()
    {
        Assert.True(true);
    }
}