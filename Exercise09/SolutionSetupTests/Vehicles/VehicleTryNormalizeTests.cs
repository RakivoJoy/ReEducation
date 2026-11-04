using SolutionSetup.Vehicles;

namespace SolutionSetupTests.Vehicles
{
    public class VehicleTryNormalizeTests
    {
        [Theory]
        [InlineData("abc123", "ABC123")]
        [InlineData("  ABC12D  ", "ABC12D")]
        public void TryNormalizeRegistrationNumber_ReturnsTrue_ForValidFormat(string input, string expected)
        {
            // Arrange & Act
            bool result = RegistrationNumber.TryNormalize(input, out string? normalized);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("AB1234")]
        [InlineData("ABI123")]
        public void TryNormalizeRegistrationNumber_ReturnsFalse_ForInvalidFormat(string? input)
        {
            // Arrange & Act
            bool result = RegistrationNumber.TryNormalize(input, out string? normalized);

            // Assert
            Assert.False(result);
            Assert.Null(normalized);
        }

        [Theory]
        [InlineData("black", "Black")]
        [InlineData("  RED  ", "Red")]
        public void TryNormalizeColor_ReturnsTrue_ForAllowedColor(string input, string expected)
        {
            // Arrange & Act
            bool result = VehicleColors.TryNormalize(input, out string? normalized);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Plaid")]
        public void TryNormalizeColor_ReturnsFalse_ForDisallowedColor(string? input)
        {
            // Arrange & Act
            bool result = VehicleColors.TryNormalize(input, out string? normalized);

            // Assert
            Assert.False(result);
            Assert.Null(normalized);
        }
    }
}
