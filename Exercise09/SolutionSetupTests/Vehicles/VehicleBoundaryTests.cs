using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleBoundaryTests
    {
        [Fact]
        public void Truck_AcceptsExactBoundaries_AndRejectsJustOutside()
        {
            // Arrange & Act
            var min = new Truck("TRK001", "Blue", 6, Truck.MinLoadCapacity);
            var max = new Truck("TRK002", "Blue", 6, Truck.MaxLoadCapacity);

            // Assert
            Assert.Equal(Truck.MinLoadCapacity, min.LoadCapacity);
            Assert.Equal(Truck.MaxLoadCapacity, max.LoadCapacity);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Truck("TRK003", "Blue", 6, Truck.MinLoadCapacity - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Truck("TRK004", "Blue", 6, Truck.MaxLoadCapacity + 1));
        }

        [Fact]
        public void Boat_AcceptsExactBoundaries_AndRejectsJustOutside()
        {
            // Arrange & Act
            var min = new Boat("BOT001", "White", 0, Boat.MinLength);
            var max = new Boat("BOT002", "White", 0, Boat.MaxLength);

            // Assert
            Assert.Equal(Boat.MinLength, min.Length);
            Assert.Equal(Boat.MaxLength, max.Length);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT003", "White", 0, Boat.MinLength - 0.01));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT004", "White", 0, Boat.MaxLength + 0.01));
        }

        [Fact]
        public void Boat_Throws_ForInfinity()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT001", "White", 0, double.PositiveInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT001", "White", 0, double.NegativeInfinity));
        }

        [Fact]
        public void Motorcycle_AcceptsExactBoundaries_AndRejectsJustOutside()
        {
            // Arrange & Act
            var min = new Motorcycle("MCY001", "Black", 2, Motorcycle.MinCylinderVolume);
            var max = new Motorcycle("MCY002", "Black", 2, Motorcycle.MaxCylinderVolume);

            // Assert
            Assert.Equal(Motorcycle.MinCylinderVolume, min.CylinderVolume);
            Assert.Equal(Motorcycle.MaxCylinderVolume, max.CylinderVolume);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Motorcycle("MCY003", "Black", 2, Motorcycle.MinCylinderVolume - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Motorcycle("MCY004", "Black", 2, Motorcycle.MaxCylinderVolume + 1));
        }

        [Fact]
        public void Airplane_AcceptsExactBoundaries_AndRejectsJustOutside()
        {
            // Arrange & Act
            var min = new Airplane("APL001", "White", 3, Airplane.MinEngines);
            var max = new Airplane("APL002", "White", 3, Airplane.MaxEngines);

            // Assert
            Assert.Equal(Airplane.MinEngines, min.NumberOfEngines);
            Assert.Equal(Airplane.MaxEngines, max.NumberOfEngines);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Airplane("APL003", "White", 3, Airplane.MinEngines - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Airplane("APL004", "White", 3, Airplane.MaxEngines + 1));
        }

        [Fact]
        public void Bus_AcceptsExactBoundaries_AndRejectsJustOutside()
        {
            // Arrange & Act
            var min = new Bus("BUS001", "Yellow", 6, Bus.MinSeats);
            var max = new Bus("BUS002", "Yellow", 6, Bus.MaxSeats);

            // Assert
            Assert.Equal(Bus.MinSeats, min.NumberOfSeats);
            Assert.Equal(Bus.MaxSeats, max.NumberOfSeats);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bus("BUS003", "Yellow", 6, Bus.MinSeats - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bus("BUS004", "Yellow", 6, Bus.MaxSeats + 1));
        }

        [Fact]
        public void ErrorMessage_IncludesRangeAndUnit()
        {
            // Arrange & Act
            var truck = Assert.Throws<ArgumentOutOfRangeException>(() => new Truck("TRK001", "Blue", 6, 0));
            var boat = Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("BOT001", "White", 0, -2));
            var motorcycle = Assert.Throws<ArgumentOutOfRangeException>(() => new Motorcycle("MCY001", "Black", 2, 10));

            // Assert
            Assert.Contains("Load capacity must be between 1 kg and 50000 kg", truck.Message);
            Assert.Contains("Length must be between 1 m and 400 m", boat.Message);
            Assert.Contains("Cylinder volume must be between 50 cc and 3000 cc", motorcycle.Message);
        }

        [Fact]
        public void Color_IsLimitedToFixedList_ForEveryVehicleType()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Truck("TRK001", "Lime", 6, 1000));
            Assert.Throws<ArgumentException>(() => new Boat("BOT001", "Lime", 0, 10));
            Assert.Throws<ArgumentException>(() => new Motorcycle("MCY001", "Lime", 2, 600));
            Assert.Throws<ArgumentException>(() => new Airplane("APL001", "Lime", 3, 2));
            Assert.Throws<ArgumentException>(() => new Bus("BUS001", "Lime", 6, 50));
        }

        [Fact]
        public void ToString_ShowsUnits_ForValidatedValues()
        {
            // Arrange
            var truck = new Truck("TRK001", "Blue", 6, 9000);
            var motorcycle = new Motorcycle("MCY001", "Black", 2, 600);

            // Act
            string truckText = truck.ToString();
            string motorcycleText = motorcycle.ToString();

            // Assert
            Assert.Contains("9000 kg", truckText);
            Assert.Contains("600 cc", motorcycleText);
        }
    }
}

