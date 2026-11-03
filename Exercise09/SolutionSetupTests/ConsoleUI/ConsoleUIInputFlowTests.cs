using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIInputFlowTests
    {
        [Fact]
        public void ParkVehicle_RejectsInvalidType_BeforeAskingForOtherDetails()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n9\n1\nABC123\nBlack\n2\n5\n600\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            string text = output.ToString();
            Assert.True(text.IndexOf("not a valid vehicle type", StringComparison.OrdinalIgnoreCase)
                < text.IndexOf("Enter registration number:", StringComparison.Ordinal));
        }

        [Fact]
        public void CreateGarage_AsksAgain_WhenCapacityIsNotANumber()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("abc\n5\n");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("Garage created with capacity 5.", output.ToString());
        }

        [Fact]
        public void CreateGarage_ShowsValidRange_WhenCapacityTooBigForInt()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("10000000000\n5\n");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("1-", output.ToString());
        }

        [Fact]
        public void ParkVehicle_AsksAgain_WhenWheelsIsNotANumber()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\nfour\n4\n10\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("Vehicle ABC123 parked.", output.ToString());
        }

        [Fact]
        public void PopulateGarage_ExplainsSamplesAlreadyAdded_WhenUsedTwice()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("10\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.PopulateGarage();

            // Assert
            Assert.Contains("already added", output.ToString());
        }

        [Fact]
        public void Run_SeparatesResultFromMenu()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n0\n");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.PressEnterToContinue, output.ToString());
        }

        [Fact]
        public void Run_ExitsCleanly_WhenInputEndsWithoutExitChoice()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.MenuTitle, output.ToString());
        }

        [Fact]
        public void CreateGarage_ReportsEndOfInput_WithoutThrowing()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("abc");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("Input ended before the garage was created.", output.ToString());
        }
    }
}
