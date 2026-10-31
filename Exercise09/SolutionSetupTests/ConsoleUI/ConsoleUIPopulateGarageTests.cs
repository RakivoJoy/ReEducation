using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIPopulateGarageTests
    {
        [Fact]
        public void PopulateGarage_ShowsSuccessMessage_WhenGarageExists()
        {
            using var input = new StringReader("10\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.PopulateGarage();

            Assert.Contains("Parked 3 of 3 vehicles.", output.ToString());
        }

        [Fact]
        public void PopulateGarage_ShowsError_WhenGarageNotCreated()
        {
            using var input = new StringReader("");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.PopulateGarage();

            Assert.Contains("No garage has been created yet.", output.ToString());
        }

        [Fact]
        public void PopulateGarage_ShowsFailureMessage_WhenCapacityExceeded()
        {
            using var input = new StringReader("1\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            ui.PopulateGarage();

            Assert.Contains("Parked 1 of 3 vehicles.", output.ToString());
        }
    }
}
