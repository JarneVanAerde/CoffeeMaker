namespace CoffeeMaker.Api.Domain;

public class RoastProfile
{
    public Guid RoastProfileId { get; set; }
    public required string RoastName { get; set; }
    public RoastLevel RoastLevel { get; set; }
    public double BeanDensity { get; set; }
    public DateTime RoastDate { get; set; }
}