using System.Globalization;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleToStringTests
    {
        [Fact]
        public void Car_ToString_IncludesSharedAndCarDetails()
        {
            // Arrange
            var vehicle = new Car("ABC123", "Black", 4, FuelType.Gasoline);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Car ABC123, Black, 4 wheels, Fuel: Gasoline", result);
        }

        [Fact]
        public void Motorcycle_ToString_IncludesCylinderVolume()
        {
            // Arrange
            var vehicle = new Motorcycle("ABC123", "Pink", 3, 600);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Motorcycle ABC123, Pink, 3 wheels, Cylinder volume: 600 cc", result);
        }

        [Fact]
        public void Airplane_ToString_IncludesNumberOfEngines()
        {
            // Arrange
            var vehicle = new Airplane("ABC123", "White", 3, 2);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Airplane ABC123, White, 3 wheels, Engines: 2", result);
        }

        [Fact]
        public void Bus_ToString_IncludesNumberOfSeats()
        {
            // Arrange
            var vehicle = new Bus("ABC123", "Yellow", 6, 50);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Bus ABC123, Yellow, 6 wheels, Seats: 50", result);
        }

        [Fact]
        public void Boat_ToString_IncludesLengthInInvariantFormat()
        {
            // Arrange
            var vehicle = new Boat("ABC123", "White", 0, 12.5);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Boat ABC123, White, 0 wheels, Length: 12.5 m", result);
        }

        [Fact]
        public void Truck_ToString_IncludesLoadCapacity()
        {
            // Arrange
            var vehicle = new Truck("ABC123", "Blue", 6, 12000);

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("Truck ABC123, Blue, 6 wheels, Load: 12000 kg", result);
        }
    }
}
