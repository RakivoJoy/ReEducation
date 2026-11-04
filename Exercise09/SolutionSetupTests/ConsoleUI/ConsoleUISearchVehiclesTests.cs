using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUISearchVehiclesTests
    {
        [Fact]
        public void SearchVehicles_FiltersByColorOnly()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader(
                "5\n" +
                "3\nABC123\nBlack\n4\n10\nGasoline\n" +
                "3\nDEF456\nRed\n4\n10\nDiesel\n" +
                "color=Black\n\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.ParkVehicle();
            ui.ParkVehicle();

            // Act
            string before = output.ToString();
            ui.SearchVehicles();

            // Assert
            string result = output.ToString()[before.Length..];
            Assert.Contains("ABC123", result);
            Assert.DoesNotContain("DEF456", result);
        }

        [Fact]
        public void SearchVehicles_CombinesMultipleCriteria()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader(
                "5\n" +
                "3\nABC123\nBlack\n4\n10\nGasoline\n" +
                "3\nDEF456\nBlack\n4\n10\nDiesel\n" +
                "color=Black wheels=4 type=Car\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.ParkVehicle();
            ui.ParkVehicle();

            // Act
            ui.SearchVehicles();

            // Assert
            string result = output.ToString();
            Assert.Contains("ABC123", result);
            Assert.Contains("DEF456", result);
        }

        [Fact]
        public void SearchVehicles_ShowsMessage_WhenNoMatches()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\ncolor=Green\n\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("No vehicles matched the given criteria.", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForInvalidWheelsInput()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\nwheels=notanumber\n\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Invalid number of wheels", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForInvalidTypeInput()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\ntype=Spaceship\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Invalid vehicle type 'Spaceship'.", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("8\n\n0\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.NoGarage, output.ToString());
        }
    }
}
