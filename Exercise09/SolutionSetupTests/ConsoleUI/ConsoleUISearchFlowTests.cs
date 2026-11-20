using SolutionSetup;
using SolutionSetup.UI;

namespace SolutionSetupTests
{
    public class ConsoleUISearchFlowTests
    {
        [Fact]
        public void SearchVehicles_DoesNotPromptForUnselectedProperties()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\ncolor=Black\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.DoesNotContain("Enter number of wheels", output.ToString());
        }

        [Fact]
        public void SearchVehicles_FindsVehicles_WhenFilteringOnOneProperty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\ncolor=Black\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Black", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForUnknownProperty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\nweight=Large\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Unknown search property 'weight'", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ShowsError_ForPairWithoutValue()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\ncolor=\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.Contains("Invalid search criterion 'color='", output.ToString());
        }

        [Fact]
        public void SearchVehicles_ListsAllVehicles_WhenCriteriaBlank()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();
            ui.PopulateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            Assert.DoesNotContain("No vehicles matched the given criteria.", output.ToString());
        }
    }
}
