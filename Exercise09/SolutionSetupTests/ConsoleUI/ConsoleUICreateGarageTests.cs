using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUICreateGarageTests
    {
        [Fact]
        public void CreateGarage_CreatesGarage_WithValidCapacity()
        {
            // Arrange
            using var input = new StringReader("5\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("Garage created with capacity 5.", output.ToString());
        }

        [Fact]
        public void CreateGarage_ShowsMessage_ForNonNumericInput()
        {
            // Arrange
            using var input = new StringReader("abc\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("Invalid capacity 'abc'", output.ToString());
        }

        [Fact]
        public void CreateGarage_ShowsMessage_ForNonPositiveCapacity()
        {
            // Arrange
            using var input = new StringReader("0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Contains("Could not create garage:", output.ToString());
        }

        [Fact]
        public void CreateGarage_AllowsRecreatingGarage()
        {
            // Arrange
            using var input = new StringReader("3\n7\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            // Act
            ui.CreateGarage();
            ui.CreateGarage();

            // Assert
            string result = output.ToString();
            Assert.Contains("Garage created with capacity 3.", result);
            Assert.Contains("Garage created with capacity 7.", result);
        }
    }
}
