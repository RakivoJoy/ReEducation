using System.Globalization;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleToStringTests
    {
        [Fact]
        public void Car_ToString_IncludesSharedAndCarDetails()
        {
            var vehicle = new Car("ABC123", "Black", 4, FuelType.Gasoline);

            Assert.Equal("Car ABC123, Black, 4 wheels, Fuel: Gasoline", vehicle.ToString());
        }

        [Fact]
        public void Motorcycle_ToString_IncludesCylinderVolume()
        {
            var vehicle = new Motorcycle("ABC123", "Pink", 3, 600);

            Assert.Equal("Motorcycle ABC123, Pink, 3 wheels, Cylinder volume: 600 cc", vehicle.ToString());
        }

        [Fact]
        public void Airplane_ToString_IncludesNumberOfEngines()
        {
            var vehicle = new Airplane("ABC123", "White", 3, 2);

            Assert.Equal("Airplane ABC123, White, 3 wheels, Engines: 2", vehicle.ToString());
        }

        [Fact]
        public void Bus_ToString_IncludesNumberOfSeats()
        {
            var vehicle = new Bus("ABC123", "Yellow", 6, 50);

            Assert.Equal("Bus ABC123, Yellow, 6 wheels, Seats: 50", vehicle.ToString());
        }

        [Fact]
        public void Boat_ToString_IncludesLengthInInvariantFormat()
        {
            var vehicle = new Boat("ABC123", "White", 0, 12.5);

            Assert.Equal("Boat ABC123, White, 0 wheels, Length: 12.5 m", vehicle.ToString());
        }

        [Fact]
        public void Truck_ToString_IncludesLoadCapacity()
        {
            var vehicle = new Truck("ABC123", "Blue", 6, 12000);

            Assert.Equal("Truck ABC123, Blue, 6 wheels, Load: 12000 kg", vehicle.ToString());
        }
    }
}
