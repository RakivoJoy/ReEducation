using System;
using System.IO;
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

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_MissingFile_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_EmptyFile_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_InvalidJson_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_UnknownVehicleType_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_InvalidVehicleData_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_MoreVehiclesThanCapacity_ReturnsFailure() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Load_DuplicateRegistrationNumbers_ReturnsFailure() { }
}
