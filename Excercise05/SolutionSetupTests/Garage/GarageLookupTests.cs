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
            var garage = new Garage(2);
            var car = CreateCar("AbC123");
            garage.AddVehicle(car);

            Assert.Same(car, garage.FindByRegistration("ABC123"));
            Assert.Same(car, garage.FindByRegistration("abc123"));
        }

        [Fact]
        public void FindByRegistration_ReturnsNull_WhenNotFound()
        {
            var garage = new Garage(2);

            Assert.Null(garage.FindByRegistration("ABC123"));
        }

        [Fact]
        public void ListAllVehicles_ReturnsOnlyParkedVehicles()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));
            garage.RemoveVehicleByRegistration("ABC123");

            var vehicles = garage.ListAllVehicles();

            Assert.Single(vehicles);
            Assert.Equal("DEF456", vehicles[0].RegistrationNumber);
        }

        [Fact]
        public void ListVehicleCountByType_CountsParkedCars()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(CreateCar("DEF456"));

            var counts = garage.ListVehicleCountByType();

            Assert.Equal(2, counts[(int)VehicleType.Car]);
            Assert.Equal(0, counts[(int)VehicleType.Bus]);
        }

        [Fact]
        public void ListVehicleCountByType_CountsMixedTypes()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123"));
            garage.AddVehicle(new Truck("DEF456", "Blue", 6, 12000));
            garage.AddVehicle(new Bus("GHI789", "Yellow", 6, 50));

            var counts = garage.ListVehicleCountByType();

            Assert.Equal(1, counts[(int)VehicleType.Car]);
            Assert.Equal(1, counts[(int)VehicleType.Truck]);
            Assert.Equal(1, counts[(int)VehicleType.Bus]);
        }
    }
}
