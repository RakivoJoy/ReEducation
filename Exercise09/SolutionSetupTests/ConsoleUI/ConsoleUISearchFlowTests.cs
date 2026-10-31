using SolutionSetup.UI;

namespace SolutionSetupTests
{
    // TODO: Phase 5 - Remove the Skip arguments once the search flow no longer steps through every property.
    // The inputs below assume the user first chooses which properties to filter on; adjust them to the final design.
    public class ConsoleUISearchFlowTests
    {
        [Fact(Skip = "Phase 5: the search still prompts for every property.")]
        public void SearchVehicles_DoesNotPromptForUnselectedProperties()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("10\nColor\nBlack\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.DoesNotContain("Enter number of wheels", output.ToString());
        }

        [Fact(Skip = "Phase 5: the search still prompts for every property.")]
        public void SearchVehicles_FindsVehicles_WhenFilteringOnOneProperty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("10\nColor\nBlack\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Black", output.ToString());
        }
    }
}
