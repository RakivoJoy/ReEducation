using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUICreateGarageTests
    {
        [Fact]
        public void CreateGarage_CreatesGarage_WithValidCapacity()
        {
            // Arrange
            var handler = new GarageHandler();
            using var input = new StringReader("5\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.True(handler.HasGarage);
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
        public void CreateGarage_DoesNotCreateGarage_ForNonPositiveCapacity()
        {
            // Arrange
            var handler = new GarageHandler();
            using var input = new StringReader("0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.False(handler.HasGarage);
        }

        [Fact]
        public void CreateGarage_AcceptsMaximumCapacity()
        {
            // Arrange
            var handler = new GarageHandler();
            using var input = new StringReader("2000\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.True(handler.HasGarage);
        }

        [Fact]
        public void CreateGarage_RePromptsForCapacityAboveMaximum()
        {
            // Arrange
            var handler = new GarageHandler();
            using var input = new StringReader("2001\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.False(handler.HasGarage);
        }

        [Fact]
        public void CreateGarage_AllowsRecreatingGarage()
        {
            // Arrange
            var handler = new GarageHandler();
            using var input = new StringReader("3\n7\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();
            ui.CreateGarage();

            // Assert
            Assert.Contains("Garage created with capacity 7.", output.ToString());
        }
    }
}
