using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using Xunit;

namespace SolutionSetupTests.Vehicles;

public class VehicleFactoryTests
{
    [Fact]
    public void Create_BuildsCarWithFuelType()
    {
        // Arrange
        var details = new Dictionary<string, object> { [VehicleFactory.FuelTypeKey] = FuelType.Diesel };

        // Act
        Vehicle vehicle = VehicleFactory.Create(VehicleType.Car, "ABC123", "Black", 4, 10, details);

        // Assert
        var car = Assert.IsType<Car>(vehicle);
        Assert.Equal(FuelType.Diesel, car.FuelType);
        Assert.Equal(10, car.ParkingSize);
    }

    [Fact]
    public void Create_BuildsBusWithSeats()
    {
        // Arrange
        var details = new Dictionary<string, object> { [VehicleFactory.NumberOfSeatsKey] = 40 };

        // Act
        Vehicle vehicle = VehicleFactory.Create(VehicleType.Bus, "BUS123", "Red", 4, 20, details);

        // Assert
        var bus = Assert.IsType<Bus>(vehicle);
        Assert.Equal(40, bus.NumberOfSeats);
    }

    [Fact]
    public void Create_ThrowsWhenRequiredDetailIsMissing()
    {
        // Arrange
        var details = new Dictionary<string, object>();

        // Act
        Action act = () => VehicleFactory.Create(VehicleType.Truck, "TRK123", "White", 6, 15, details);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}
