using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleTypesTests
    {
        [Fact]
        public void Motorcycle_HasMotorcycleTypeAndCylinderVolume()
        {
            var vehicle = new Motorcycle("ABC123", "Pink", 3, 600);

            Assert.Equal(VehicleType.Motorcycle, vehicle.Type);
            Assert.Equal(600, vehicle.CylinderVolume);
        }

        [Fact]
        public void Airplane_HasAirplaneTypeAndNumberOfEngines()
        {
            var vehicle = new Airplane("ABC123", "White", 3, 2);

            Assert.Equal(VehicleType.Airplane, vehicle.Type);
            Assert.Equal(2, vehicle.NumberOfEngines);
        }

        [Fact]
        public void Bus_HasBusTypeAndNumberOfSeats()
        {
            var vehicle = new Bus("ABC123", "Yellow", 6, 50);

            Assert.Equal(VehicleType.Bus, vehicle.Type);
            Assert.Equal(50, vehicle.NumberOfSeats);
        }

        [Fact]
        public void Boat_HasBoatTypeAndLength()
        {
            var vehicle = new Boat("ABC123", "White", 0, 12.5);

            Assert.Equal(VehicleType.Boat, vehicle.Type);
            Assert.Equal(12.5, vehicle.Length);
        }

        [Fact]
        public void Truck_HasTruckTypeAndLoadCapacity()
        {
            var vehicle = new Truck("ABC123", "Blue", 6, 12000);

            Assert.Equal(VehicleType.Truck, vehicle.Type);
            Assert.Equal(12000, vehicle.LoadCapacity);
        }

        [Fact]
        public void Truck_ThrowsForNegativeLoadCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Truck("ABC123", "Blue", 6, -1));
        }
    }
}
