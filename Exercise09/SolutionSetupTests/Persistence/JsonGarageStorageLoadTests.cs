using System;
using System.IO;
using System.Threading.Tasks;
using SolutionSetup.Persistence;
using Xunit;

namespace SolutionSetupTests;

[Collection(FileIOCollection.Name)]
public class JsonGarageStorageLoadTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public JsonGarageStorageLoadTests() => Directory.CreateDirectory(_folder);

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    private string FilePath => Path.Combine(_folder, "garage.json");

    private static string CarJson(string registrationNumber) =>
        $$"""{"kind":"Car","registrationNumber":"{{registrationNumber}}","color":"Black","numberOfWheels":4,"parkingSize":5,"fuelType":"Gasoline"}""";

    private static string GarageJson(int capacity, params string[] vehicles) =>
        $$"""{"version":1,"capacity":{{capacity}},"vehicles":[{{string.Join(",", vehicles)}}]}""";

    private async Task WriteFileAsync(string content) => await File.WriteAllTextAsync(FilePath, content);

    [Fact]
    public async Task Load_ValidFile_ReturnsGarage()
    {
        // Arrange
        await WriteFileAsync(GarageJson(5, CarJson("ABC123")));

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(5, result.Garage!.Capacity);
        Assert.Single(result.Garage);
    }

    [Fact]
    public async Task Load_MissingFile_ReturnsFailure()
    {
        // Arrange
        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_EmptyFile_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync(string.Empty);

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_InvalidJson_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync("{ not json");

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_UnknownVehicleType_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync(GarageJson(5, CarJson("ABC123").Replace("\"Car\"", "\"Spaceship\"")));

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_InvalidVehicleData_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync(GarageJson(5, CarJson("NOPE")));

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_MoreVehiclesThanCapacity_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync(GarageJson(1, CarJson("ABC123"), CarJson("DEF456")));

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }

    [Fact]
    public async Task Load_DuplicateRegistrationNumbers_ReturnsFailure()
    {
        // Arrange
        await WriteFileAsync(GarageJson(5, CarJson("ABC123"), CarJson("ABC123")));

        // Act
        GarageLoadResult result = await new JsonGarageStorage(FilePath).LoadAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Garage);
    }
}

