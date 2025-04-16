using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Infrastructure;

namespace CoffeeMaker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoffeeMakerDbContext>();
        builder.Services.AddScoped<CoffeeBrewingCalculator>();
        
        var app = builder.Build();
       
        app.MapGet("setup", async (CoffeeMakerDbContext dbContext) =>
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        });
        
        app.MapPost("api/brew-recommendation", async(BrewingRecommendationRequest request, CoffeeBrewingCalculator calculator) =>
        {
            var recommendation = await calculator.CalculateBrewingRecommendation(request);
            return Results.Ok(recommendation);
        });
        
        app.Run();
    }
}
