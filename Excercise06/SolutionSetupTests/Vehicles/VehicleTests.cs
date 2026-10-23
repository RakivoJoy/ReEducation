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

        [Theory]
        [InlineData("AB123")]
        [InlineData("ABCD123")]
        [InlineData("ABC12")]
        [InlineData("ABC1234")]
        [InlineData("123ABC")]
        [InlineData("ABC-123")]
        [InlineData("AB1C23")]
        public void Constructor_ThrowsForRegistrationNumberNotInFormatABC123(string registrationNumber)
        {
            Assert.Throws<ArgumentException>(() => new Car(registrationNumber, "Black", 4, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_NormalizesRegistrationNumberToUppercase()
        {
            var vehicle = new Car("  abc123  ", "Black", 4, FuelType.Gasoline);

            Assert.Equal("ABC123", vehicle.RegistrationNumber);
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
