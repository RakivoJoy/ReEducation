using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleTests
    {
        [Fact]
        public void Constructor_ThrowsForEmptyRegistrationNumber()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("", "Black", 4, FuelType.Gasoline));
            Assert.Throws<ArgumentException>(() => new Car("   ", "Black", 4, FuelType.Gasoline));
        }

        [Theory]
        [InlineData("ABC123")]
        [InlineData("ABC12D")]
        public void Constructor_AcceptsRegistrationNumberInAllowedFormat(string registrationNumber)
        {
            // Arrange & Act
            var vehicle = new Car(registrationNumber, "Black", 4, FuelType.Gasoline);

            // Assert
            Assert.Equal(registrationNumber, vehicle.RegistrationNumber);
        }

        [Theory]
        [InlineData("AB123")]
        [InlineData("ABCD123")]
        [InlineData("ABC12")]
        [InlineData("ABC1234")]
        [InlineData("123ABC")]
        [InlineData("ABC-123")]
        [InlineData("AB1C23")]
        [InlineData("ABC1-2")]
        [InlineData("ABI123")]
        [InlineData("ABQ123")]
        [InlineData("ABV123")]
        [InlineData("ABC12I")]
        [InlineData("ABC12Q")]
        [InlineData("ABC12V")]
        public void Constructor_ThrowsForRegistrationNumberNotInAllowedFormat(string registrationNumber)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Car(registrationNumber, "Black", 4, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_NormalizesRegistrationNumberToUppercase()
        {
            // Arrange & Act
            var vehicle = new Car("  abc123  ", "Black", 4, FuelType.Gasoline);

            // Assert
            Assert.Equal("ABC123", vehicle.RegistrationNumber);
        }

        [Fact]
        public void Constructor_ThrowsForEmptyColor()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("ABC123", "", 4, FuelType.Gasoline));
        }

        [Theory]
        [InlineData("Purple Rain")]
        [InlineData("Invisible")]
        public void Constructor_ThrowsForColorNotInAllowedList(string color)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("ABC123", color, 4, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_NormalizesColorToAllowedCasing()
        {
            // Arrange & Act
            var vehicle = new Car("ABC123", "burgundy", 4, FuelType.Gasoline);

            // Assert
            Assert.Equal("Burgundy", vehicle.Color);
        }

        [Fact]
        public void Constructor_ThrowsForNegativeNumberOfWheels()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car("ABC123", "Black", -1, FuelType.Gasoline));
        }

        [Fact]
        public void Constructor_AcceptsZeroWheels()
        {
            // Arrange & Act
            var vehicle = new Car("ABC123", "Black", 0, FuelType.Gasoline);

            // Assert
            Assert.Equal(0, vehicle.NumberOfWheels);
        }
    }
}
