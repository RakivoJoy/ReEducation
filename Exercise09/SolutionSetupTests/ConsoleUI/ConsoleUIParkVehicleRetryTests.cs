using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIParkVehicleRetryTests
    {
        [Fact]
        public void ParkVehicle_RepromptsRegistration_WhenRegistrationInvalid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nAB1234\nABC123\nBlack\n4\n10\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.NotNull(handler.FindVehicle("ABC123"));
            Assert.Contains("Invalid registration number 'AB1234'", output.ToString());
        }

        [Fact]
        public void ParkVehicle_RepromptsColor_WhenColorInvalid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nInvisible\nBlack\n4\n10\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Equal("Black", handler.FindVehicle("ABC123")?.Color);
            Assert.Contains("Invalid color 'Invisible'", output.ToString());
        }

        [Fact]
        public void ParkVehicle_DoesNotRepromptRegistration_WhenOnlyColorInvalid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nXYZ999\nInvisible\nRed\n4\n10\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Equal("Red", handler.FindVehicle("XYZ999")?.Color);
            Assert.DoesNotContain("Invalid registration number", output.ToString());
        }

        [Fact]
        public void ParkVehicle_EndsInput_WhenRegistrationNeverValid()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nAB1234\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Null(handler.FindVehicle("AB1234"));
            Assert.Contains(ConsoleUIText.InputEndedBeforeVehicleParked, output.ToString());
        }
    }
}
