using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIParkVehicleTests
    {
        [Fact]
        public void ParkVehicle_ParksCar_WhenInputValid()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Vehicle ABC123 parked.", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ParksTruck_WhenInputValid()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n6\nTRK001\nBlue\n6\n9000\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Vehicle TRK001 parked.", output.ToString());
        }

        [Fact]
        public void ParkVehicle_OffersFiveMostCommonColors()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Most common colors: Black, White, Silver, Gray, Red", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsError_ForColorNotInAllowedList()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nInvisible\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Could not park vehicle:", output.ToString());
            Assert.Contains("Invisible", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsError_WhenNoGarageCreated()
        {
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            ui.ParkVehicle();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsError_ForInvalidWheelsInput()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\nnotanumber\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Invalid number of wheels.", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsError_ForInvalidTypeSelection()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n99\nABC123\nBlack\n4\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Invalid input:", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsError_ForInvalidFuelType()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nElectric\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();

            Assert.Contains("Invalid input:", output.ToString());
        }

        [Fact]
        public void ParkVehicle_ShowsFailureMessage_ForDuplicateRegistration()
        {
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n3\nABC123\nRed\n4\nDiesel\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.ParkVehicle();
            ui.ParkVehicle();

            Assert.Contains("already parked", output.ToString());
        }
    }
}
