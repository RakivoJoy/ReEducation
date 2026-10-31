using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIListTests
    {
        [Fact]
        public void ListVehicles_PrintsVehicles_WhenGarageHasVehicles()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Contains("ABC123", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsMessage_WhenGarageIsEmpty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Contains("No vehicles are parked in the garage.", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Contains("No garage has been created yet.", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_PrintsCounts_WhenGarageHasVehicles()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            // Act
            ui.ListVehicleCounts();

            // Assert
            Assert.Contains("Car: 1", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_ShowsMessage_WhenGarageIsEmpty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ListVehicleCounts();

            // Assert
            Assert.Contains("No vehicles are parked in the garage.", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.ListVehicleCounts();

            // Assert
            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
