using CoffeeMaker.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api.Infrastructure;

public class CoffeeDbContext(DbContextOptions<CoffeeDbContext> options) : DbContext(options)
{
    public DbSet<RoastProfile> RoastProfiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoastProfile>()
            .HasIndex(r => r.RoastName)
            .IsUnique();
    }
}