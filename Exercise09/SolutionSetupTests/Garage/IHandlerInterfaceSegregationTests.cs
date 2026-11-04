using SolutionSetup;
using SolutionSetup.Vehicles;
using System.Collections.Generic;
using Xunit;

namespace SolutionSetupTests.Garage;

public class IHandlerInterfaceSegregationTests
{
    [Fact]
    public void GarageHandler_CanBeUsedThroughEachFocusedInterface()
    {
        // Arrange
        var handler = new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider());
        IGarageLifecycle lifecycle = handler;
        IVehicleOperations operations = handler;
        IVehicleQueries queries = handler;
        IGaragePersistence persistence = handler;

        // Act
        OperationResult created = lifecycle.CreateGarage(3);
        OperationResult parked = operations.ParkVehicle(TestVehicles.CreateCar("XYZ111"));
        IReadOnlyList<IVehicle> vehicles = queries.ListVehicles();
        OperationResult saved = persistence.SaveGarage();

        // Assert
        Assert.True(created.Success);
        Assert.True(lifecycle.HasGarage);
        Assert.True(parked.Success);
        Assert.Single(vehicles);
        Assert.True(saved.Success);
    }
}
