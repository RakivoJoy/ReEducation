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
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));

            var results = garage.SearchByProperties(new SearchCriteria { RegistrationNumber = "abc123" });

            Assert.Single(results);
            Assert.Equal("ABC123", results[0].RegistrationNumber);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenNoVehicleMatches()
        {
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));

            var results = garage.SearchByProperties(new SearchCriteria { Color = "Green" });

            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenCriteriaContradict()
        {
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.AddVehicle(CreateCar("DEF456", "Red"));

            var results = garage.SearchByProperties(new SearchCriteria
            {
                RegistrationNumber = "DEF456",
                Color = "Black"
            });

            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_FiltersByNumberOfWheelsOnly()
        {
            var garage = new Garage(3);
            garage.AddVehicle(new Motorcycle("ABC123", "Blue", 2, 600));
            garage.AddVehicle(CreateCar("DEF456"));
            garage.AddVehicle(new Truck("GHL789", "Blue", 6, 8000));

            var results = garage.SearchByProperties(new SearchCriteria { NumberOfWheels = 2 });

            Assert.Single(results);
            Assert.Equal(VehicleType.Motorcycle, results[0].Type);
        }

        [Fact]
        public void SearchByProperties_ReturnsEmpty_WhenGarageIsEmpty()
        {
            var garage = new Garage(2);

            var results = garage.SearchByProperties(new SearchCriteria { Type = VehicleType.Car });

            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ExcludesRemovedVehicles()
        {
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.RemoveVehicleByRegistration("ABC123");

            var results = garage.SearchByProperties(new SearchCriteria { Color = "Black" });

            Assert.Empty(results);
        }

        [Fact]
        public void SearchByProperties_ThrowsForNullCriteria()
        {
            var garage = new Garage(2);

            Assert.Throws<ArgumentNullException>(() => garage.SearchByProperties(null!));
        }
    }
}
