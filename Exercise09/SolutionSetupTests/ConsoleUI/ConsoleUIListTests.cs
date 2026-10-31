using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIListTests
    {
        [Fact]
        public void ListVehicles_PrintsVehicles_WhenGarageHasVehicles()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            ui.ListVehicles();

            Assert.Contains("ABC123", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsMessage_WhenGarageIsEmpty()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ListVehicles();

            Assert.Contains("No vehicles are parked in the garage.", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsError_WhenNoGarageCreated()
        {
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            ui.ListVehicles();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_PrintsCounts_WhenGarageHasVehicles()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            ui.ListVehicleCounts();

            Assert.Contains("Car: 1", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_ShowsMessage_WhenGarageIsEmpty()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ListVehicleCounts();

            Assert.Contains("No vehicles are parked in the garage.", output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_ShowsError_WhenNoGarageCreated()
        {
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            ui.ListVehicleCounts();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
