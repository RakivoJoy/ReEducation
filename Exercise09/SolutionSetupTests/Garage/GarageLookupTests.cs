using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GarageLookupTests
    {
        [Fact]
        public void FindByRegistration_IsCaseInsensitive()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            var car = CreateCar("AbC123");
            garage.AddVehicle(car);

            // Act & Assert
            Assert.Same(car, garage.FindByRegistration("ABC123"));
            Assert.Same(car, garage.FindByRegistration("abc123"));
        }

        [Fact]
        public void FindByRegistration_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act
            var result = garage.FindByRegistration("ABC123");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ListAllVehicles_ReturnsOnlyParkedVehicles()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));
            garage.RemoveVehicleByRegistration("ABC123");

            // Act
            var vehicles = garage.ListAllVehicles();

            // Assert
            Assert.Single(vehicles);
            Assert.Equal("DEF456", vehicles[0].RegistrationNumber);
        }

        [Fact]
        public void ListVehicleCountByType_CountsParkedCars()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));

            // Act
            var counts = garage.ListVehicleCountByType();

            // Assert
            Assert.Equal(2, counts[VehicleType.Car]);
            Assert.False(counts.ContainsKey(VehicleType.Bus));
        }

        [Fact]
        public void ListVehicleCountByType_CountsMixedTypes()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(new Truck("DEF456", "Blue", 6, 12000));
            garage.AddVehicle(new Bus("GHL789", "Yellow", 6, 50));

            // Act
            var counts = garage.ListVehicleCountByType();

            // Assert
            Assert.Equal(1, counts[VehicleType.Car]);
            Assert.Equal(1, counts[VehicleType.Truck]);
            Assert.Equal(1, counts[VehicleType.Bus]);
        }
    }
}
