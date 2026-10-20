using SolutionSetup;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class GarageTests
    {
        private static Car CreateCar(string registrationNumber, string color = "Black", FuelType fuelType = FuelType.Gasoline) =>
            new(registrationNumber, color, 4, fuelType);

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

            Assert.Equal(2, counts[VehicleType.Car]);
            Assert.False(counts.ContainsKey(VehicleType.Bus));
        }

        [Fact]
        public void SearchByProperties_FiltersByColor()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.AddVehicle(CreateCar("DEF456", "Red"));
            garage.AddVehicle(CreateCar("GHI789", "black"));

            var results = garage.SearchByProperties(new SearchCriteria { Color = "Black" });

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void SearchByProperties_CombinesMultipleCriteria()
        {
            var garage = new Garage(3);
            garage.AddVehicle(CreateCar("ABC123", "Black", FuelType.Gasoline));
            garage.AddVehicle(CreateCar("DEF456", "Black", FuelType.Diesel));
            garage.AddVehicle(CreateCar("GHI789", "Red", FuelType.Diesel));

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
    }
}
