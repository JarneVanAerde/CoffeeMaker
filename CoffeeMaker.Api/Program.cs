using CoffeeMaker.Api;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CoffeeDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CoffeeMakerDb;Trusted_Connection=True;"));
builder.Services.AddSingleton<CoffeeBrewingCalculator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/brew-recommendation/{roastProfileId:int}", async (BrewingRecommendationRequest request, CoffeeBrewingCalculator calculator) =>
    {
        var recommendation = await calculator.CalculateBrewingRecommendation(request);
        return Results.Ok(recommendation);
    });

app.Run();
