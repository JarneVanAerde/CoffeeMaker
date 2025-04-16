using CoffeeMaker.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMaker.Api.Infrastructure;

public class CoffeeMakerDbContext(DbContextOptions<CoffeeMakerDbContext> options) : DbContext(options)
{
    public DbSet<RoastProfile> RoastProfiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoastProfile>()
            .HasIndex(r => r.RoastName)
            .IsUnique();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(
                "Server=localhost,1433;Database=CoffeeMakerDb;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;TrustServerCertificate=True;")
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var noRoastProfiles = !context.Set<RoastProfile>().Any();
                if (noRoastProfiles)
                {
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

                    await context.Set<RoastProfile>().AddRangeAsync(roastProfiles, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
            });
    }
}