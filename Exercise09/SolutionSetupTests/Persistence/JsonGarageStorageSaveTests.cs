using System;
using System.IO;
using System.Threading.Tasks;
using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using Xunit;

namespace SolutionSetupTests;

[Collection(FileIOCollection.Name)]
public class JsonGarageStorageSaveTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public JsonGarageStorageSaveTests() => Directory.CreateDirectory(_folder);

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    private string FilePath => Path.Combine(_folder, "garage.json");

    private static Garage<IVehicle> GarageWith(string registrationNumber)
    {
        Garage<IVehicle> garage = new(5);
        garage.AddVehicle(TestVehicles.CreateCar(registrationNumber));
        return garage;
    }

    [Fact]
    public async Task Save_WritesFile()
    {
        // Arrange
        JsonGarageStorage storage = new(FilePath);

        // Act
        OperationResult result = await storage.SaveAsync(GarageWith("ABC123"));

        // Assert
        Assert.True(result.Success);
        Assert.Contains("\"registrationNumber\": \"ABC123\"", await File.ReadAllTextAsync(FilePath));
    }

    [Fact]
    public async Task Save_CreatesMissingDirectory()
    {
        // Arrange
        string path = Path.Combine(_folder, "nested", "garage.json");

        // Act
        OperationResult result = await new JsonGarageStorage(path).SaveAsync(GarageWith("ABC123"));

        // Assert
        Assert.True(result.Success);
        Assert.True(File.Exists(path));
    }

    [Fact]
    public async Task Save_OverwritesExistingFile_KeepsBackupAndLeavesNoTempFile()
    {
        // Arrange
        JsonGarageStorage storage = new(FilePath);
        await storage.SaveAsync(GarageWith("ABC123"));

        // Act
        OperationResult result = await storage.SaveAsync(GarageWith("DEF456"));

        // Assert
        Assert.True(result.Success);
        Assert.Contains("DEF456", await File.ReadAllTextAsync(FilePath));
        Assert.Contains("ABC123", await File.ReadAllTextAsync(FilePath + ".bak"));
        Assert.False(File.Exists(FilePath + ".tmp"));
    }

    [Fact]
    public async Task Save_ReturnsFailure_WhenPathIsDirectory_AndLeavesNoTempFile()
    {
        // Arrange
        Directory.CreateDirectory(FilePath);

        // Act
        OperationResult result = await new JsonGarageStorage(FilePath).SaveAsync(GarageWith("ABC123"));

        // Assert
        Assert.False(result.Success);
        Assert.False(File.Exists(FilePath + ".tmp"));
    }
}
