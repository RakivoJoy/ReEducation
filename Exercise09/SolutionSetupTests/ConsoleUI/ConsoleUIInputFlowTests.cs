using SolutionSetup.UI;

namespace SolutionSetupTests
{
    // TODO: Phase 4 - Remove the Skip arguments once input is validated immediately and the user flow is improved.
    public class ConsoleUIInputFlowTests
    {
        [Fact(Skip = "Phase 4: the vehicle type is validated after all prompts.")]
        public void ParkVehicle_RejectsInvalidType_BeforeAskingForOtherDetails()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n9\n1\nABC123\nBlack\n2\n600\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            string text = output.ToString();
            Assert.True(text.IndexOf("not a valid vehicle type", StringComparison.OrdinalIgnoreCase)
                < text.IndexOf("Enter registration number:", StringComparison.Ordinal));
        }

        [Fact(Skip = "Phase 4: invalid input does not re-prompt yet.")]
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

        [Fact(Skip = "Phase 4: the valid range is not shown yet.")]
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

        [Fact(Skip = "Phase 4: invalid wheels input does not re-prompt yet.")]
        public void ParkVehicle_AsksAgain_WhenWheelsIsNotANumber()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\nfour\n4\nGasoline\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("Vehicle ABC123 parked.", output.ToString());
        }

        [Fact(Skip = "Phase 4: populate twice gives no explanation yet.")]
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

        [Fact(Skip = "Phase 4: the menu is printed directly after the result.")]
        public void Run_SeparatesResultFromMenu()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n0\n");
            var ui = new ConsoleUI(input, output);

            // Act
            ui.Run();

            // Assert
            Assert.Contains("Press Enter to continue", output.ToString());
        }
    }
}
