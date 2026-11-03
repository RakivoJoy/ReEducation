using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIFindVehicleTests
    {
        [Fact]
        public void FindVehicle_PrintsVehicle_WhenFound()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n10\nGasoline\nABC123\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();
            ui.ParkVehicle();

            // Act
            ui.FindVehicle();

            // Assert
            Assert.Contains("ABC123", output.ToString());
        }

        [Fact]
        public void FindVehicle_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(5);
            using var output = new StringWriter();
            var ui = new ConsoleUI(new StringReader("ZZZ999\n"), output, handler);

            // Act
            ui.FindVehicle();

            // Assert
            Assert.Null(handler.FindVehicle("ZZZ999"));
        }

        [Fact]
        public void FindVehicle_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("7\n\n0\n");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.NoGarage, output.ToString());
        }
    }
}
