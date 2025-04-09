namespace CoffeeMaker.Api.Contracts;

public class BrewingRecommendationRequest
{
    public required string RoastName { get; set; }
    public required string Method { get; set; }
    public double DesiredStrength { get; set; } // Scale 1 (weak) - 10 (strong)
    public DateTime BrewDate { get; set; }
}

