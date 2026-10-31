using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GarageAdvancedSearchTests
    {
        [Fact]
        public void SearchByProperties_MatchesRegistrationNumberCaseInsensitively()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { RegistrationNumber = "abc123" });

            // Assert
            Assert.Single(results);
            Assert.Equal("ABC123", results[0].RegistrationNumber);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenNoVehicleMatches()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { Color = "Green" });

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenCriteriaContradict()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.AddVehicle(CreateCar("DEF456", "Red"));

            // Act
            var results = garage.SearchByProperties(new SearchCriteria
            {
                RegistrationNumber = "DEF456",
                Color = "Black"
            });

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_FiltersByNumberOfWheelsOnly()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(new Motorcycle("ABC123", "Blue", 2, 600));
            garage.AddVehicle(CreateCar("DEF456"));
            garage.AddVehicle(new Truck("GHL789", "Blue", 6, 8000));

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { NumberOfWheels = 2 });

            // Assert
            Assert.Single(results);
            Assert.Equal(VehicleType.Motorcycle, results[0].Type);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenGarageIsEmpty()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { Type = VehicleType.Car });

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ExcludesRemovedVehicles()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.RemoveVehicleByRegistration("ABC123");

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { Color = "Black" });

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ThrowsForNullCriteria()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => garage.SearchByProperties(null!));
        }
    }
}
