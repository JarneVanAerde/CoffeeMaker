namespace CoffeeMaker.IntegrationTests.Dtos;

public class RoastProfileDto
{
    public required string RoastName { get; set; }
    public required string RoastLevel { get; set; }
    public double BeanDensity { get; set; }
    public DateTime RoastDate { get; set; }
}