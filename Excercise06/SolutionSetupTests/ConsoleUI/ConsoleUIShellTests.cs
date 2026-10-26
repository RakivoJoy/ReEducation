using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUIShellTests
    {
        [Fact]
        public void Run_PrintsMenu_AndExitsOnZero()
        {
            using var input = new StringReader("0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.Run();

            string result = output.ToString();
            Assert.Contains("Garage 2.0", result);
            Assert.Contains("0. Exit", result);
        }

        [Fact]
        public void Run_ShowsMessage_AndContinues_ForInvalidChoice()
        {
            using var input = new StringReader("99\n0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.Run();

            string result = output.ToString();
            Assert.Contains("Invalid choice. Please select a valid menu option.", result);
        }

        [Fact]
        public void Run_PrintsMenuTwice_WhenInvalidChoiceThenExit()
        {
            using var input = new StringReader("abc\n0\n");
            using var output = new StringWriter();
            var ui = new ConsoleUI(input, output);

            ui.Run();

            string result = output.ToString();
            int occurrences = result.Split("Garage 2.0").Length - 1;
            Assert.Equal(2, occurrences);
        }
    }
}
