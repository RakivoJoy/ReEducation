using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleColorsTests
    {
        [Fact]
        public void All_ContainsTwentyUniqueColors()
        {
            // Arrange & Act
            var all = VehicleColors.All;

            // Assert
            Assert.Equal(20, all.Count);
            Assert.Equal(20, all.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }

        [Fact]
        public void MostCommon_ContainsFirstFiveAllowedColors()
        {
            // Arrange & Act
            var mostCommon = VehicleColors.MostCommon;

            // Assert
            Assert.Equal(5, mostCommon.Count);
            Assert.Equal(VehicleColors.All.Take(5), mostCommon);
        }

        [Theory]
        [InlineData("Black", "Black")]
        [InlineData("black", "Black")]
        [InlineData("  BURGUNDY ", "Burgundy")]
        public void Normalize_ReturnsAllowedColorWithCanonicalCasing(string input, string expected)
        {
            // Arrange & Act
            var result = VehicleColors.Normalize(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Purple Rain")]
        [InlineData("Chartreuse")]
        [InlineData("Invisible")]
        public void Normalize_ThrowsForColorNotInList(string color)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => VehicleColors.Normalize(color));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Normalize_ThrowsForEmptyOrWhiteSpace(string color)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => VehicleColors.Normalize(color));
        }

        [Fact]
        public void Normalize_ThrowsForNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => VehicleColors.Normalize(null!));
        }
    }
}
