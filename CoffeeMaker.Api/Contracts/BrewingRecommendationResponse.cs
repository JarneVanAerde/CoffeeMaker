namespace CoffeeMaker.Api.Contracts;

public class BrewingRecommendationResponse
{
    public double RecommendedTemperatureCelsius { get; set; }
    public TimeSpan RecommendedExtractionTime { get; set; }
}