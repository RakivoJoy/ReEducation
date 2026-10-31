using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    // TODO: Phase 2 - Remove the Skip arguments once each vehicle type validates its own values.
    // The limits used here are proposals; adjust them to the ranges chosen in Phase 2.
    public class VehicleTypeValidationTests
    {
        private const string SkipReason = "Phase 2: vehicle-specific validation is not implemented yet.";

        [Theory(Skip = SkipReason)]
        [InlineData(1)]
        [InlineData(9000)]
        public void Truck_AcceptsValidLoadCapacity(int loadCapacity)
        {
            // Arrange & Act
            var truck = new Truck("TRK001", "Blue", 6, loadCapacity);

            // Assert
            Assert.Equal(loadCapacity, truck.LoadCapacity);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void Truck_Throws_ForLoadCapacityOutsideRange(int loadCapacity)
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Truck("TRK001", "Blue", 6, loadCapacity));
            Assert.Contains("kg", ex.Message);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(1.5)]
        [InlineData(30)]
        public void Boat_AcceptsValidLength(double length)
        {
            // Arrange & Act
            var boat = new Boat("BOT001", "White", 0, length);

            // Assert
            Assert.Equal(length, boat.Length);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(100000)]
        [InlineData(double.NaN)]
        public void Boat_Throws_ForLengthOutsideRange(double length)
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT001", "White", 0, length));
            Assert.Contains("m", ex.Message);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(50)]
        [InlineData(1800)]
        public void Motorcycle_AcceptsValidCylinderVolume(int cylinderVolume)
        {
            // Arrange & Act
            var motorcycle = new Motorcycle("MCY001", "Black", 2, cylinderVolume);

            // Assert
            Assert.Equal(cylinderVolume, motorcycle.CylinderVolume);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void Motorcycle_Throws_ForCylinderVolumeOutsideRange(int cylinderVolume)
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Motorcycle("MCY001", "Black", 2, cylinderVolume));
            Assert.Contains("cc", ex.Message);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(1)]
        [InlineData(4)]
        public void Airplane_AcceptsValidNumberOfEngines(int engines)
        {
            // Arrange & Act
            var airplane = new Airplane("AIR001", "White", 3, engines);

            // Assert
            Assert.Equal(engines, airplane.NumberOfEngines);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(-3)]
        [InlineData(0)]
        [InlineData(1000)]
        public void Airplane_Throws_ForNumberOfEnginesOutsideRange(int engines)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Airplane("AIR001", "White", 3, engines));
        }

        [Theory(Skip = SkipReason)]
        [InlineData(1)]
        [InlineData(80)]
        public void Bus_AcceptsValidNumberOfSeats(int seats)
        {
            // Arrange & Act
            var bus = new Bus("BUS001", "Yellow", 6, seats);

            // Assert
            Assert.Equal(seats, bus.NumberOfSeats);
        }

        [Theory(Skip = SkipReason)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1000)]
        public void Bus_Throws_ForNumberOfSeatsOutsideRange(int seats)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bus("BUS001", "Yellow", 6, seats));
        }
    }
}
