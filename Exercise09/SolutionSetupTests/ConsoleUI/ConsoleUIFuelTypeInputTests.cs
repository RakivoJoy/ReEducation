using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIFuelTypeInputTests
    {
        [Theory]
        [InlineData("g", "Gasoline")]
        [InlineData("G", "Gasoline")]
        [InlineData("d", "Diesel")]
        [InlineData("D", "Diesel")]
        [InlineData("Gasoline", "Gasoline")]
        [InlineData("gasoline", "Gasoline")]
        [InlineData("DIESEL", "Diesel")]
        public void ParkCar_AcceptsFuelTypeByFirstLetterOrFullName(string fuelInput, string expectedFuel)
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader($"5\n3\nABC123\nBlack\n4\n10\n{fuelInput}\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains(expectedFuel, output.ToString());
        }

        [Fact]
        public void ParkCar_ReportsInvalidFuelType_ForUnknownLetter()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n10\nx\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("Invalid input: 'x' is not a valid fuel type.", output.ToString());
        }

        [Fact]
        public void ParkCar_RePromptsForFuelType_AfterInvalidInput()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n10\nx\ng\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("Invalid input: 'x' is not a valid fuel type.", output.ToString());
            Assert.Contains("Vehicle ABC123 parked.", output.ToString());
        }
    }
}
