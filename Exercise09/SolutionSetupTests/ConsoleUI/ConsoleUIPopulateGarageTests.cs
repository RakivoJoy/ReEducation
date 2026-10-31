using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIPopulateGarageTests
    {
        [Fact]
        public void PopulateGarage_ShowsSuccessMessage_WhenGarageExists()
        {
            // Arrange
            using var input = new StringReader("10\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.PopulateGarage();

            // Assert
            Assert.Contains("Parked 3 of 3 vehicles.", output.ToString());
        }

        [Fact]
        public void PopulateGarage_ShowsError_WhenGarageNotCreated()
        {
            // Arrange
            using var input = new StringReader("");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            // Act
            ui.PopulateGarage();

            // Assert
            Assert.Contains(ConsoleUIText.NoGarage, output.ToString());
        }

        [Fact]
        public void PopulateGarage_ShowsFailureMessage_WhenCapacityExceeded()
        {
            // Arrange
            using var input = new StringReader("1\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.PopulateGarage();

            // Assert
            Assert.Contains("Parked 1 of 3 vehicles.", output.ToString());
        }
    }
}
