using SolutionSetup;
using SolutionSetup.Vehicles;
using System.Collections.Generic;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GaragePopulateTests
    {
        [Fact]
        public void Populate_ParksAllVehicles_WhenCapacityAllows()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);

            // Act
            var result = garage.Populate(SampleData.CreateSampleVehicles());

            // Assert
            Assert.True(result.Success);
            Assert.Equal(3, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void Populate_ReturnsFailure_WhenCapacityExceeded()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act
            var result = garage.Populate(SampleData.CreateSampleVehicles());

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Parked 2 of 3", result.Message);
            Assert.Equal(2, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void Populate_ReturnsFailure_WhenRegistrationNumberDuplicated()
        {
            var garage = new Garage<IVehicle>(3);
            var vehicles = new List<Vehicle>
            {
                CreateCar("ABC123"),
                CreateCar("abc123")
            };

            // Act
            var result = garage.Populate(vehicles);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Parked 1 of 2", result.Message);
        }

        [Fact]
        public void Populate_ThrowsForNull()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => garage.Populate(null!));
        }
    }
}
