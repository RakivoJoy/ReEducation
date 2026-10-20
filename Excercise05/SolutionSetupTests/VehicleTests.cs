using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleTests
    {
        [Fact]
        public void Constructor_ThrowsForEmptyRegistrationNumber()
        {
            Assert.Throws<ArgumentException>(() => new Car("", "Black", 4, FuelType.Gasoline));
            Assert.Throws<ArgumentException>(() => new Car("   ", "Black", 4, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_ThrowsForEmptyColor()
        {
            Assert.Throws<ArgumentException>(() => new Car("ABC123", "", 4, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_ThrowsForNegativeNumberOfWheels()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car("ABC123", "Black", -1, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_AcceptsZeroWheels()
        {
            var vehicle = new Car("ABC123", "Black", 0, FuelType.Gasoline);

            Assert.Equal(0, vehicle.NumberOfWheels);
        }
    }
}
