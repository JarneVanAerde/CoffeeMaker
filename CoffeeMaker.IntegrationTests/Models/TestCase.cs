using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;

namespace CoffeeMaker.IntegrationTests.Models;

public class TestCase
{
    public required string Name { get; set; }
    
    public required BrewingRecommendationRequest Request { get; set; }
    public required BrewingRecommendationResponse Response { get; set; }
    public required List<RoastProfile> RoastProfiles { get; set; }

    public override string ToString()
    {
        return Name;
    }
}