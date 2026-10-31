using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    // TODO: Phase 3 - Remove the Skip arguments once the menus are built from the VehicleType enum.
    public class ConsoleUIVehicleTypeMenuTests
    {
        [Fact(Skip = "Phase 3: the park menu is still hard-coded.")]
        public void ParkVehicleMenu_ListsEveryVehicleType()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                Assert.Contains(type.ToString(), output.ToString());
            }
        }

        [Fact(Skip = "Phase 3: the search prompt is still hard-coded.")]
        public void SearchPrompt_ListsEveryVehicleType()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n\n\n\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                Assert.Contains(type.ToString(), output.ToString());
            }
        }

        [Theory(Skip = "Phase 3: the park switch is still hard-coded.")]
        [InlineData("1", "Motorcycle")]
        [InlineData("2", "Airplane")]
        [InlineData("3", "Car")]
        [InlineData("4", "Bus")]
        [InlineData("5", "Boat")]
        [InlineData("6", "Truck")]
        public void ParkVehicleMenu_NumbersMatchEnumOrder(string choice, string expectedName)
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains($"{choice}. {expectedName}", output.ToString());
        }
    }
}
