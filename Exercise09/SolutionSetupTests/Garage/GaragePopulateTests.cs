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
            Assert.Empty(result.Failures);
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
            Assert.Equal(3, result.Total);
            Assert.Equal(2, result.Parked);
            Assert.Single(result.Failures);
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
            Assert.Equal(2, result.Total);
            Assert.Equal(1, result.Parked);
            Assert.Equal(ParkFailureReason.Duplicate, result.Failures[0].Reason);
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
