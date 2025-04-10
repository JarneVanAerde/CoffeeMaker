using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoffeeMakerDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CoffeeMakerDb;Trusted_Connection=True;"));

        builder.Services.AddScoped<CoffeeBrewingCalculator>();

        var app = builder.Build();
        
        app.MapPost("/brew-recommendation/{roastProfileId:int}", async (BrewingRecommendationRequest request, CoffeeBrewingCalculator calculator) =>
        {
            var recommendation = await calculator.CalculateBrewingRecommendation(request);
            return Results.Ok(recommendation);
        });
        
        app.Run();
    }
}
