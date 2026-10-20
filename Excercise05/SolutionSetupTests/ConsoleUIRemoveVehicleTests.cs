using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIRemoveVehicleTests
    {
        [Fact]
        public void RemoveVehicle_RemovesVehicle_WhenFound()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\nABC123\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.ParkVehicle();

            ui.RemoveVehicle();

            Assert.Contains("Vehicle ABC123 removed.", output.ToString());
        }

        [Fact]
        public void RemoveVehicle_ShowsMessage_WhenNotFound()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\nZZZ999\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.RemoveVehicle();

            Assert.Contains("No vehicle with registration number ZZZ999 was found.", output.ToString());
        }

        [Fact]
        public void RemoveVehicle_ShowsError_WhenNoGarageCreated()
        {
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            ui.RemoveVehicle();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
