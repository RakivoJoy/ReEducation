using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GarageCapacityTests
    {
        [Fact]
        public void Constructor_ThrowsForNonPositiveCapacity()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<IVehicle>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<IVehicle>(-1));
        }

        [Fact]
        public void Constructor_ThrowsForCapacityAboveMaximum()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<IVehicle>(Garage<IVehicle>.MaxCapacity + 1));
        }

        [Fact]
        public void Constructor_AcceptsMaximumCapacity()
        {
            // Arrange
            var garage = new Garage<IVehicle>(Garage<IVehicle>.MaxCapacity);

            // Act & Assert
            Assert.Equal(Garage<IVehicle>.MaxCapacity, garage.Capacity);
        }

        [Fact]
        public void Capacity_ReturnsConstructorValue()
        {
            // Arrange
            var garage = new Garage<IVehicle>(5);

            // Act & Assert
            Assert.Equal(5, garage.Capacity);
        }

        [Fact]
        public void AddVehicle_ReturnsSuccess_WhenSpaceAvailable()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act
            var result = garage.AddVehicle(CreateCar("ABC123"));

            // Assert
            Assert.True(result.Success);
            Assert.Contains("ABC123", result.Message);
        }

        [Fact]
        public void AddVehicle_ReturnsFailure_WhenGarageFull()
        {
            var garage = new Garage<IVehicle>(1);
            // Arrange
            garage.AddVehicle(CreateCar("ABC123"));

            // Act
            var result = garage.AddVehicle(CreateCar("DEF456"));

            // Assert
            Assert.False(result.Success);
            Assert.Contains("full", result.Message);
        }

        [Fact]
        public void AddVehicle_ReturnsFailure_WhenRegistrationNumberAlreadyExists()
        {
            var garage = new Garage<IVehicle>(2);
            // Arrange
            garage.AddVehicle(CreateCar("ABC123"));

            // Act
            var result = garage.AddVehicle(CreateCar("abc123"));

            // Assert
            Assert.False(result.Success);
            Assert.Contains("already", result.Message);
        }

        [Fact]
        public void AddVehicle_ThrowsForNull()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => garage.AddVehicle(null!));
        }

        [Fact]
        public void RemoveVehicleByRegistration_ReturnsSuccess_AndFreesSpace()
        {
            var garage = new Garage<IVehicle>(1);
            // Arrange
            garage.AddVehicle(CreateCar("ABC123"));

            // Act
            var result = garage.RemoveVehicleByRegistration("ABC123");

            // Assert
            Assert.True(result.Success);
            Assert.True(garage.AddVehicle(CreateCar("DEF456")).Success);
        }

        [Fact]
        public void RemoveVehicleByRegistration_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act
            var result = garage.RemoveVehicleByRegistration("ABC123");

            // Assert
            Assert.False(result.Success);
            Assert.Contains("ABC123", result.Message);
        }
    }
}
