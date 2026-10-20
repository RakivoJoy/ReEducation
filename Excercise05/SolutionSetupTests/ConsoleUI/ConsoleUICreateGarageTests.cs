using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUICreateGarageTests
    {
        [Fact]
        public void CreateGarage_CreatesGarage_WithValidCapacity()
        {
            using var input = new StringReader("5\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.CreateGarage();

            Assert.Contains("Garage created with capacity 5.", output.ToString());
        }

        [Fact]
        public void CreateGarage_ShowsMessage_ForNonNumericInput()
        {
            using var input = new StringReader("abc\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.CreateGarage();

            Assert.Contains("Invalid capacity 'abc'", output.ToString());
        }

        [Fact]
        public void CreateGarage_ShowsMessage_ForNonPositiveCapacity()
        {
            using var input = new StringReader("0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.CreateGarage();

            Assert.Contains("Could not create garage:", output.ToString());
        }

        [Fact]
        public void CreateGarage_AllowsRecreatingGarage()
        {
            using var input = new StringReader("3\n7\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.CreateGarage();
            ui.CreateGarage();

            string result = output.ToString();
            Assert.Contains("Garage created with capacity 3.", result);
            Assert.Contains("Garage created with capacity 7.", result);
        }
    }
}
