using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GarageSearchTests
    {
        [Fact]
        public void SearchByProperties_FiltersByColor()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.AddVehicle(CreateCar("DEF456", "Red"));
            garage.AddVehicle(CreateCar("GHL789", "black"));

            var results = garage.SearchByProperties(new SearchCriteria { Color = "Black" });

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void SearchByProperties_CombinesMultipleCriteria()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123", "Black", FuelType.Gasoline));
            garage.AddVehicle(CreateCar("DEF456", "Black", FuelType.Diesel));
            garage.AddVehicle(CreateCar("GHL789", "Red", FuelType.Diesel));

            var results = garage.SearchByProperties(new SearchCriteria
            {
                Color = "Black",
                Type = VehicleType.Car,
                NumberOfWheels = 4
            });

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void SearchByProperties_WithEmptyCriteria_ReturnsAllVehicles()
        {
            var garage = new Garage(2);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));

            var results = garage.SearchByProperties(new SearchCriteria());

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void SearchByProperties_FiltersByType()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(new Truck("DEF456", "Blue", 6, 12000));
            garage.AddVehicle(new Truck("GHL789", "Red", 6, 8000));

            var results = garage.SearchByProperties(new SearchCriteria { Type = VehicleType.Truck });

            Assert.Equal(2, results.Count);
            Assert.All(results, vehicle => Assert.Equal(VehicleType.Truck, vehicle.Type));
        }

        [Fact]
        public void SearchByProperties_CombinesColorTypeAndWheels()
        {
            var garage = new Garage(3);
            garage.AddVehicle(new Motorcycle("ABC123", "Pink", 3, 600));
            garage.AddVehicle(new Motorcycle("DEF456", "Pink", 2, 600));
            garage.AddVehicle(CreateCar("GHL789", "Pink"));

            var results = garage.SearchByProperties(new SearchCriteria
            {
                Color = "Pink",
                Type = VehicleType.Motorcycle,
                NumberOfWheels = 3
            });

            Assert.Single(results);
            Assert.Equal("ABC123", results[0].RegistrationNumber);
        }
    }
}
