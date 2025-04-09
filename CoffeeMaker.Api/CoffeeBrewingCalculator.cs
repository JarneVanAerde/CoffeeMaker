using System.ComponentModel;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api;

public class CoffeeBrewingCalculator(CoffeeMakerDbContext context)
{
    public async Task<BrewingRecommendationResponse> CalculateBrewingRecommendation(BrewingRecommendationRequest request)
    {
        var roastProfile = await context.RoastProfiles
            .FirstOrDefaultAsync(r => r.RoastName == request.RoastName);

        if (roastProfile == null)
            throw new InvalidOperationException("Roast profile not found.");

        if (!Enum.TryParse<BrewingMethod>(request.Method, ignoreCase: true, out var brewingMethod))
            throw new InvalidEnumArgumentException($"Brewing method {request.Method} not found.");
        
        double baseTemperature = brewingMethod switch
        {
            BrewingMethod.Espresso => 92,
            BrewingMethod.PourOver => 94,
            BrewingMethod.FrenchPress => 95,
            BrewingMethod.AeroPress => 90,
            _ => 93
        };

        double roastAdjustment = roastProfile.RoastLevel switch
        {
            RoastLevel.Light => 2,
            RoastLevel.Medium => 0,
            RoastLevel.Dark => -2,
            _ => 0
        };

        int daysSinceRoast = (request.BrewDate - roastProfile.RoastDate).Days;
        double freshnessAdjustment = daysSinceRoast > 14 ? 1 : 0;

        double finalTemperature = baseTemperature + roastAdjustment + freshnessAdjustment;

        double baseTimeSeconds = brewingMethod switch
        {
            BrewingMethod.Espresso => 25,
            BrewingMethod.PourOver => 180,
            BrewingMethod.FrenchPress => 240,
            BrewingMethod.AeroPress => 90,
            _ => 180
        };

        double densityAdjustment = roastProfile.BeanDensity < 0.68 ? -15 : 0;
        double strengthAdjustment = (request.DesiredStrength - 5) * 5;

        double finalTimeSeconds = baseTimeSeconds + densityAdjustment + strengthAdjustment;

        return new BrewingRecommendationResponse
        {
            RecommendedTemperatureCelsius = Math.Round(finalTemperature, 1),
            RecommendedExtractionTime = TimeSpan.FromSeconds(finalTimeSeconds)
        };
    }
}