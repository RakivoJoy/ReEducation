using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIFindVehicleTests
    {
        [Fact]
        public void FindVehicle_PrintsVehicle_WhenFound()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\nABC123\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            // Act
            ui.FindVehicle();

            // Assert
            Assert.Contains("ABC123", output.ToString());
        }

        [Fact]
        public void FindVehicle_ShowsMessage_WhenNotFound()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\nZZZ999\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.FindVehicle();

            // Assert
            Assert.Contains("No vehicle with registration number ZZZ999 was found.", output.ToString());
        }

        [Fact]
        public void FindVehicle_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.FindVehicle();

            // Assert
            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
