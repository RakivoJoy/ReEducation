using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleColorsTests
    {
        [Fact]
        public void All_ContainsTwentyUniqueColors()
        {
            Assert.Equal(20, VehicleColors.All.Count);
            Assert.Equal(20, VehicleColors.All.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }

        [Fact]
        public void MostCommon_ContainsFirstFiveAllowedColors()
        {
            Assert.Equal(5, VehicleColors.MostCommon.Count);
            Assert.Equal(VehicleColors.All.Take(5), VehicleColors.MostCommon);
        }

        [Theory]
        [InlineData("Black", "Black")]
        [InlineData("black", "Black")]
        [InlineData("  BURGUNDY ", "Burgundy")]
        public void Normalize_ReturnsAllowedColorWithCanonicalCasing(string input, string expected)
        {
            Assert.Equal(expected, VehicleColors.Normalize(input));
        }

        [Theory]
        [InlineData("Purple Rain")]
        [InlineData("Chartreuse")]
        [InlineData("Invisible")]
        public void Normalize_ThrowsForColorNotInList(string color)
        {
            Assert.Throws<ArgumentException>(() => VehicleColors.Normalize(color));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Normalize_ThrowsForEmptyOrWhiteSpace(string color)
        {
            Assert.Throws<ArgumentException>(() => VehicleColors.Normalize(color));
        }

        [Fact]
        public void Normalize_ThrowsForNull()
        {
            Assert.Throws<ArgumentNullException>(() => VehicleColors.Normalize(null!));
        }
    }
}
