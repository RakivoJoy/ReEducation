using SolutionSetup;
using SolutionSetup.Vehicles;
using System.Collections.Generic;
using Xunit;

namespace SolutionSetupTests;

public class GarageHandlerSampleDataTests
{
    private sealed class FakeSampleDataProvider : ISampleDataProvider
    {
        public IReadOnlyList<IVehicle> CreateSampleVehicles() => [TestVehicles.CreateCar("XYZ111")];
    }

    [Fact]
    public void PopulateGarage_UsesInjectedSampleData()
    {
        // Arrange
        var handler = new GarageHandler(new InMemoryGarageStorage(), new FakeSampleDataProvider());
        handler.CreateGarage(5);

        // Act
        OperationResult result = handler.PopulateGarage();

        // Assert
        Assert.True(result.Success);
        IReadOnlyList<IVehicle> vehicles = handler.ListVehicles();
        Assert.Single(vehicles);
        Assert.Equal("XYZ111", vehicles[0].RegistrationNumber);
    }
}
