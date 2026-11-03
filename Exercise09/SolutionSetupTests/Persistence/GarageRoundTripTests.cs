using System;
using System.IO;
using Xunit;

namespace SolutionSetupTests;

[Collection(FileIOCollection.Name)]
public class GarageRoundTripTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public GarageRoundTripTests() => Directory.CreateDirectory(_folder);

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void SaveThenLoad_PreservesCapacityAndAllVehicleTypes() { }
}
