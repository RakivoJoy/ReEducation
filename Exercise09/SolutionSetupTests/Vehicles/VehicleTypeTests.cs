using SolutionSetup;
using SolutionSetup.Vehicles;
using Xunit;

namespace SolutionSetupTests.Vehicles;

public class VehicleTypeTests
{
    [Fact]
    public void Constructor_SetsBusBeforeParkingSizeValidation()
    {
        // Arrange
        var bus = new Bus("ABC123", "red", 4, 30);

        // Act
        VehicleType type = bus.Type;

        // Assert
        Assert.Equal(VehicleType.Bus, type);
        Assert.Equal(ParkingSizeLimits.For(VehicleType.Bus).Min, bus.ParkingSize);
    }


    //TODO: Create type + parkingSize tests for all vehcile types to test new constructor with type parameter.

}
