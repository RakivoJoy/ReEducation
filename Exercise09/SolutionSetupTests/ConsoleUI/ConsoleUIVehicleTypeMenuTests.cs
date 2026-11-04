using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIVehicleTypeMenuTests
    {
        [Fact]
        public void ParkVehicleMenu_ListsEveryVehicleType()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage()));
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                Assert.Contains(type.ToString(), output.ToString());
            }
        }

        [Fact]
        public void SearchPrompt_ListsEveryVehicleType()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage()));
            ui.CreateGarage();

            // Act
            ui.SearchVehicles();

            // Assert
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                Assert.Contains(type.ToString(), output.ToString());
            }
        }

        [Theory]
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
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage()));
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains($"{choice}. {expectedName}", output.ToString());
        }
    }
}
