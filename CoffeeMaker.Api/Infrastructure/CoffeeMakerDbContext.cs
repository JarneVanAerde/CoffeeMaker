using CoffeeMaker.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api.Infrastructure;

public class CoffeeMakerDbContext(DbContextOptions<CoffeeMakerDbContext> options) : DbContext(options)
{
    public DbSet<RoastProfile> RoastProfiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoastProfile>()
            .HasNoKey()
            .HasIndex(r => r.RoastName)
            .IsUnique();
    }
}