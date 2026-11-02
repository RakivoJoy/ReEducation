using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIHandlerTests
    {
        private sealed class FakeHandler : IHandler
        {
            public int? CreatedCapacity { get; private set; }
            public bool HasGarage => CreatedCapacity is not null;

            public OperationResult CreateGarage(int capacity)
            {
                CreatedCapacity = capacity;
                return OperationResult.Ok("fake created");
            }

            public OperationResult PopulateGarage() => OperationResult.Ok("fake populated");
            public OperationResult ParkVehicle(IVehicle vehicle) => OperationResult.Ok($"fake parked {vehicle.RegistrationNumber}");
            public OperationResult RemoveVehicle(string registrationNumber) => OperationResult.Ok("fake removed");
            public IReadOnlyList<IVehicle> ListVehicles() => [];
            public Dictionary<VehicleType, int> ListVehicleCounts() => [];
            public IVehicle? FindVehicle(string registrationNumber) => null;
            public IReadOnlyList<IVehicle> SearchVehicles(SearchCriteria criteria) => [];
        }

        [Fact]
        public void CreateGarage_UsesHandler_AndPrintsItsMessage()
        {
            // Arrange
            var handler = new FakeHandler();
            using var output = new StringWriter();
            using var input = new StringReader("7\n");
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.CreateGarage();

            // Assert
            Assert.Equal(7, handler.CreatedCapacity);
            Assert.Contains("fake created", output.ToString());
        }

        [Fact]
        public void PopulateGarage_PrintsHandlerMessage()
        {
            // Arrange
            var handler = new FakeHandler();
            handler.CreateGarage(3);
            using var output = new StringWriter();
            using var input = new StringReader(string.Empty);
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.PopulateGarage();

            // Assert
            Assert.Contains("fake populated", output.ToString());
        }

        [Fact]
        public void ParkVehicle_SendsVehicleToHandler()
        {
            // Arrange
            var handler = new FakeHandler();
            handler.CreateGarage(3);
            using var output = new StringWriter();
            using var input = new StringReader("3\nABC123\nBlack\n4\n10\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.ParkVehicle();

            // Assert
            Assert.Contains("fake parked ABC123", output.ToString());
        }

        [Fact]
        public void ListVehicles_ShowsNoGarageMessage_WhenHandlerHasNoGarage()
        {
            // Arrange
            var handler = new FakeHandler();
            using var output = new StringWriter();
            using var input = new StringReader(string.Empty);
            var ui = new ConsoleUI(input, output, handler);

            // Act
            ui.ListVehicles();

            // Assert
            Assert.Contains("No garage has been created yet", output.ToString());
        }
    }
}
