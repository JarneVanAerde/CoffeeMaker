using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;
using CoffeeMaker.IntegrationTests.Dtos;
using CoffeeMaker.IntegrationTests.Models;
using Xunit.Sdk;

namespace CoffeeMaker.IntegrationTests.Setup;

public class FileDataAttribute(string baseFolderPath) : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        ArgumentNullException.ThrowIfNull(testMethod);

        var cases = GetTestCases(baseFolderPath).GetAwaiter().GetResult();

        foreach (var item in cases)
        {
            yield return new object[] { item };
        }
    }

    private static async Task<K?> GetJsonContent<K>(DirectoryInfo directory, string fileName) where K : class
    {
        var settings = new JsonSerializerOptions
        {
            RespectRequiredConstructorParameters = false,
            Converters = { new JsonStringEnumConverter() }
        };

        var path = Path.Combine(directory.FullName, fileName);
        if (File.Exists(path))
        {
            using FileStream openStream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<K>(openStream, settings);
        }
        
        if (directory.Parent != null)
            return GetJsonContent<K>(directory.Parent, fileName).Result;

        return default;
    }

    private static async Task<List<RoastProfile>> GetRoastProfiles(DirectoryInfo directory)
    {
        var dtoResults = await GetJsonContent<IEnumerable<RoastProfileDto>>(directory, "roast-profiles.json");

        return dtoResults!.Select(dto => 
            new RoastProfile
            {
                RoastName = dto.RoastName,
                BeanDensity = dto.BeanDensity,
                RoastDate = dto.RoastDate,
                RoastLevel = Enum.Parse<RoastLevel>(dto.RoastLevel)
            }).ToList();
    }

    private async Task<List<TestCase>> GetTestCases(string path)
    {
        var cases = new List<TestCase>();
        var directoryInfo = new DirectoryInfo(path);
        
        foreach (var directory in directoryInfo.GetDirectories())
        {
            if (directory.GetDirectories().Length == 0)
            {
                var request = await GetJsonContent<BrewingRecommendationRequest>(directory, "request.json");
                var response = await GetJsonContent<BrewingRecommendationResponse>(directory, "response.json");

                var warehouses = await GetRoastProfiles(directory);

                cases.Add(new TestCase
                {
                    Name = directory.Name,
                    RoastProfiles = warehouses,
                    Request = request!,
                    Response = response!,
                });
            }
            else
            {
                cases.AddRange(await GetTestCases(directory.FullName));
            }
        }

        return cases;
    }
}