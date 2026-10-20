using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUISearchVehiclesTests
    {
        [Fact]
        public void SearchVehicles_FiltersByColorOnly()
        {
            using var output = new StringWriter();
            using var input = new StringReader(
                "5\n" +
                "3\nABC123\nBlack\n4\nGasoline\n" +
                "3\nDEF456\nRed\n4\nDiesel\n" +
                "\nBlack\n\n\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();
            ui.ParkVehicle();

            string before = output.ToString();
            ui.SearchVehicles();

            string result = output.ToString()[before.Length..];
            Assert.Contains("ABC123", result);
            Assert.DoesNotContain("DEF456", result);
        }

        [Fact]
        public void SearchVehicles_CombinesMultipleCriteria()
        {
            using var output = new StringWriter();
            using var input = new StringReader(
                "5\n" +
                "3\nABC123\nBlack\n4\nGasoline\n" +
                "3\nDEF456\nBlack\n4\nDiesel\n" +
                "\nBlack\n4\nCar\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();
            ui.ParkVehicle();

            ui.SearchVehicles();

            string result = output.ToString();
            Assert.Contains("ABC123", result);
            Assert.Contains("DEF456", result);
        }

        [Fact]
        public void SearchVehicles_ShowsMessage_WhenNoMatches()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n\nGreen\n\n\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.SearchVehicles();

            Assert.Contains("No vehicles matched the given criteria.", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForInvalidWheelsInput()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n\nnotanumber\n\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.SearchVehicles();

            Assert.Contains("Invalid number of wheels", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForInvalidTypeInput()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n\n\nSpaceship\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.SearchVehicles();

            Assert.Contains("Invalid vehicle type 'Spaceship'.", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_WhenNoGarageCreated()
        {
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            ui.SearchVehicles();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
