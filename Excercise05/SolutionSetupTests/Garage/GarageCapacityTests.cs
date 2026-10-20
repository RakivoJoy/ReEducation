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
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage(-1));
        }

        [Fact]
        public void Capacity_ReturnsConstructorValue()
        {
            var garage = new Garage(5);

            Assert.Equal(5, garage.Capacity);
        }

        [Fact]
        public void AddVehicle_ReturnsSuccess_WhenSpaceAvailable()
        {
            var garage = new Garage(1);

            var result = garage.AddVehicle(CreateCar("ABC123"));

            Assert.True(result.Success);
            Assert.Contains("ABC123", result.Message);
        }

        [Fact]
        public void AddVehicle_ReturnsFailure_WhenGarageFull()
        {
            var garage = new Garage(1);
            garage.AddVehicle(CreateCar("ABC123"));

            var result = garage.AddVehicle(CreateCar("DEF456"));

            Assert.False(result.Success);
            Assert.Contains("full", result.Message);
        }

        [Fact]
        public void AddVehicle_ReturnsFailure_WhenRegistrationNumberAlreadyExists()
        {
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123"));

            var result = garage.AddVehicle(CreateCar("abc123"));

            Assert.False(result.Success);
            Assert.Contains("already", result.Message);
        }

        [Fact]
        public void AddVehicle_ThrowsForNull()
        {
            var garage = new Garage(1);

            Assert.Throws<ArgumentNullException>(() => garage.AddVehicle(null!));
        }

        [Fact]
        public void RemoveVehicleByRegistration_ReturnsSuccess_AndFreesSpace()
        {
            var garage = new Garage(1);
            garage.AddVehicle(CreateCar("ABC123"));

            var result = garage.RemoveVehicleByRegistration("ABC123");

            Assert.True(result.Success);
            Assert.True(garage.AddVehicle(CreateCar("DEF456")).Success);
        }

        [Fact]
        public void RemoveVehicleByRegistration_ReturnsFailure_WhenNotFound()
        {
            var garage = new Garage(1);

            var result = garage.RemoveVehicleByRegistration("ABC123");

            Assert.False(result.Success);
            Assert.Contains("ABC123", result.Message);
        }
    }
}
