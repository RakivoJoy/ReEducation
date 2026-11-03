using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIEndToEndTests
    {
        [Fact]
        public void Run_CreatesGarageAndParksCar_ThenExits()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("1\n5\n\n3\n3\nABC123\nBlack\n4\n10\nGasoline\n\n0\n");
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.Run();

            // Assert
            Assert.NotNull(handler.FindVehicle("ABC123"));
            Assert.Contains(ConsoleUIText.MenuTitle, output.ToString());
        }

        [Fact]
        public void Run_ParksRemovesAndSearches_InOneSession()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader(
                "1\n5\n\n" +
                "3\n3\nABC123\nBlack\n4\n10\nGasoline\n\n" +
                "8\ncolor=Black\n\n" +
                "4\nABC123\n\n" +
                "0\n");
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.Run();

            // Assert
            Assert.Contains("ABC123", output.ToString());
            Assert.Null(handler.FindVehicle("ABC123"));
        }

        [Fact]
        public void Run_RecoversFromInvalidChoicesAndFieldErrors_ThenParksAndExits()
        {
            // Arrange
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader(
                "9\n" +
                "1\n5\n\n" +
                "3\n3\nAB1234\nABC123\nInvisible\nRed\n4\n10\nGasoline\n\n" +
                "7\nABC123\n\n" +
                "0\n");
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.Run();

            // Assert
            Assert.Equal("Red", handler.FindVehicle("ABC123")?.Color);
            Assert.Contains(ConsoleUIText.InvalidChoice, output.ToString());
            Assert.Contains("Invalid registration number 'AB1234'", output.ToString());
            Assert.Contains("Invalid color 'Invisible'", output.ToString());
        }
    }
}
