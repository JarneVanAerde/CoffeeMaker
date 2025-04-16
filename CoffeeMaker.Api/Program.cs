using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoffeeMakerDbContext>(options =>
        {
            options.UseSqlServer(
                "Server=localhost,1433;Database=CoffeeMakerDb;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;TrustServerCertificate=True;");
        });
        builder.Services.AddScoped<CoffeeBrewingCalculator>();
        
        var app = builder.Build();
       
        app.MapGet("seed", async (CoffeeMakerDbContext dbContext, CancellationToken cancellationToken) =>
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            
            var roastProfiles = new[]
            {
                new RoastProfile
                {
                    RoastName = "Midnight Ember",
                    RoastLevel = RoastLevel.Dark,
                    BeanDensity = 0.65,
                    RoastDate = new DateTime(2025, 4, 5, 9, 30, 0)
                },
                new RoastProfile
                {
                    RoastName = "Golden Bloom",
                    RoastLevel = RoastLevel.Light,
                    BeanDensity = 0.72,
                    RoastDate = new DateTime(2025, 3, 28, 14, 45, 0)
                },
                new RoastProfile
                {
                    RoastName = "Velvet Dusk",
                    RoastLevel = RoastLevel.Medium,
                    BeanDensity = 0.68,
                    RoastDate = new DateTime(2025, 4, 1, 11, 0, 0)
                },
                new RoastProfile
                {
                    RoastName = "Morning Whisper",
                    RoastLevel = RoastLevel.Light,
                    BeanDensity = 0.74,
                    RoastDate = new DateTime(2025, 3, 22, 8, 15, 0)
                },
                new RoastProfile
                {
                    RoastName = "Crimson Peak",
                    RoastLevel = RoastLevel.Medium,
                    BeanDensity = 0.70,
                    RoastDate = new DateTime(2025, 4, 8, 16, 20, 0)
                }
            };
    
            await dbContext.Set<RoastProfile>().AddRangeAsync(roastProfiles, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        });
        
        app.MapPost("api/brew-recommendation", async(BrewingRecommendationRequest request, CoffeeBrewingCalculator calculator) =>
        {
            var recommendation = await calculator.CalculateBrewingRecommendation(request);
            return Results.Ok(recommendation);
        });
        
        app.Run();
    }
}
