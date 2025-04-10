using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;

namespace CoffeeMaker.IntegrationTests.Models;

public class TestCase
{
    public required string Name { get; set; }
    
    public BrewingRecommendationRequest Request { get; set; }
    public BrewingRecommendationResponse Response { get; set; }
    public List<RoastProfile> RoastProfiles { get; set; }

    public override string ToString()
    {
        return Name;
    }
}