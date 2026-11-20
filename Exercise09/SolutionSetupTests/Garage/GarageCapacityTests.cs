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
            Assert.True(result.Parked);
            Assert.Null(result.Reason);
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
            Assert.False(result.Parked);
            Assert.Equal(ParkFailureReason.Full, result.Reason);
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
            Assert.False(result.Parked);
            Assert.Equal(ParkFailureReason.Duplicate, result.Reason);
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
            Assert.True(result.Removed);
            Assert.True(garage.AddVehicle(CreateCar("DEF456")).Parked);
        }

        [Fact]
        public void RemoveVehicleByRegistration_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act
            var result = garage.RemoveVehicleByRegistration("ABC123");

            // Assert
            Assert.False(result.Removed);
            Assert.Equal("ABC123", result.RegistrationNumber);
        }
    }
}
