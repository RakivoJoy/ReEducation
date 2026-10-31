using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIParkVehicleTests
    {
        [Fact]
        public void ParkVehicle_ParksCar_WhenInputValid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.NotNull(handler.FindVehicle("ABC123"));
        }

        [Fact]
        public void ParkVehicle_ParksTruck_WhenInputValid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n6\nTRK001\nBlue\n6\n9000\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.IsType<Truck>(handler.FindVehicle("TRK001"));
        }

        [Fact]
        public void ParkVehicle_OffersFiveMostCommonColors()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("Most common colors: Black, White, Silver, Gray, Red", output.ToString());
        }

        [Fact]
        public void ParkVehicle_DoesNotPark_ForColorNotInAllowedList()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nInvisible\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Null(handler.FindVehicle("ABC123"));
        }

        [Fact]
        public void ParkVehicle_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("No garage has been created yet.", output.ToString());
        }

        [Fact]
        public void ParkVehicle_DoesNotPark_ForInvalidWheelsInput()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\nnotanumber\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Empty(handler.ListVehicles());
        }

        [Fact]
        public void ParkVehicle_DoesNotPark_ForInvalidTypeSelection()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n99\nABC123\nBlack\n4\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Empty(handler.ListVehicles());
        }

        [Fact]
        public void ParkVehicle_DoesNotPark_ForInvalidFuelType()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nElectric\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Empty(handler.ListVehicles());
        }

        [Fact]
        public void ParkVehicle_ShowsFailureMessage_ForDuplicateRegistration()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\nGasoline\n3\nABC123\nRed\n4\nDiesel\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();
            ui.ParkVehicle();

            // Assert
            Assert.Contains("already parked", output.ToString());
        }
    }
}
