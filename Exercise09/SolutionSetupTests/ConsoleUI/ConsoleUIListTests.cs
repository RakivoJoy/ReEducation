using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIListTests
    {
        [Fact]
        public void ListVehicles_PrintsVehicles_WhenGarageHasVehicles()
        {
            // Arrange
            using var output = new StringWriter();
            var handler = new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider());
            handler.CreateGarage(5);
            handler.ParkVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            var ui = new ConsoleUI(new StringReader(""), output, handler);

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Single(handler.ListVehicles());
            Assert.Contains("ABC123", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsMessage_WhenGarageIsEmpty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Contains(ConsoleUIText.NoVehiclesParked, output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n\n0\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));

            // Act
            ui.Run();

            // Assert
            Assert.Contains(ConsoleUIText.NoGarage, output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_CountsParkedVehiclesByType()
        {
            // Arrange
            using var output = new StringWriter();
            var handler = new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider());
            handler.CreateGarage(5);
            handler.ParkVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            var ui = new ConsoleUI(new StringReader(""), output, handler);

            // Act
            ui.ListVehicleCounts();

            // Assert
            Assert.Equal(1, handler.ListVehicleCounts()[VehicleType.Car]);
        }

        [Fact]
        public void ListVehicleCounts_ShowsMessage_WhenGarageIsEmpty()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("5\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));
            ui.CreateGarage();

            // Act
            ui.ListVehicleCounts();

            // Assert
            Assert.Contains(ConsoleUIText.NoVehiclesParked, output.ToString());
        }

        [Fact]
        public void ListVehicleCounts_ShowsError_WhenNoGarageCreated()
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader("6\n\n0\n");
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider()));

            // Act
            ui.Run();

            // Assert
            Assert.Contains("No garage has been created yet.", output.ToString());
        }
    }
}
