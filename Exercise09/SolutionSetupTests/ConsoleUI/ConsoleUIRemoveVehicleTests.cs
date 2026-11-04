using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIRemoveVehicleTests
    {
        [Fact]
        public void RemoveVehicle_RemovesVehicle_WhenFound()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n10\nGasoline\nABC123\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.ParkVehicle();

            // Act
            ui.RemoveVehicle();

            // Assert
            Assert.Contains("Vehicle ABC123 removed.", output.ToString());
        }

        [Fact]
        public void RemoveVehicle_ShowsMessage_WhenNotFound()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\nZZZ999\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.RemoveVehicle();

            // Assert
            Assert.Contains("No vehicle with registration number ZZZ999 was found.", output.ToString());
        }

        [Fact]
        public void RemoveVehicle_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("4\n\n0\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.NoGarage, output.ToString());
        }
    }
}
