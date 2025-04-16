using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using CoffeeMaker.Api.Contracts;
using CoffeeMaker.Api.Domain;
using CoffeeMaker.IntegrationTests.Dtos;
using CoffeeMaker.IntegrationTests.Models;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace CoffeeMaker.IntegrationTests.Setup;

public class FileDataAttribute(string baseFolderPath) : DataAttribute
{
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
    {
        var cases = await GetTestCases(baseFolderPath);

        return new ReadOnlyCollection<TheoryDataRow>(cases.Select(x => new TheoryDataRow(x)).ToList());
    }

    public override bool SupportsDiscoveryEnumeration()
    {
        return false;
    }

    private static async Task<T?> GetJsonContent<T>(DirectoryInfo directory, string fileName) where T : class
    {
        var path = Path.Combine(directory.FullName, fileName);
        if (File.Exists(path))
        {
            await using var openStream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(openStream);
        }
        
        if (directory.Parent != null)
            return GetJsonContent<T>(directory.Parent, fileName).Result;

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
        
        var roastProfiles = await GetRoastProfiles(directoryInfo);
        
        foreach (var directory in directoryInfo.GetDirectories())
        {
            if (directory.GetDirectories().Length == 0)
            {
                var request = await GetJsonContent<BrewingRecommendationRequest>(directory, "request.json");
                var response = await GetJsonContent<BrewingRecommendationResponse>(directory, "response.json");
                
                cases.Add(new TestCase
                {
                    Name = directory.Name,
                    RoastProfiles = roastProfiles,
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