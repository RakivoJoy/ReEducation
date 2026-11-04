using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ConsoleUIVehicleTypeRegistryTests
    {
        [Fact]
        public void VehicleTypes_HasExactlyOneEntryPerEnumValue()
        {
            // Arrange
            var ui = new ConsoleUI(new StringReader(""), new StringWriter(), new GarageHandler(new InMemoryGarageStorage()));

            // Act
            var registered = ui.VehicleTypes.Select(info => info.Type).ToList();

            // Assert
            Assert.Equal(Enum.GetValues<VehicleType>().OrderBy(t => t), registered.OrderBy(t => t));
        }

        [Theory]
        [InlineData(VehicleType.Motorcycle, "600\n")]
        [InlineData(VehicleType.Airplane, "2\n")]
        [InlineData(VehicleType.Car, "Gasoline\n")]
        [InlineData(VehicleType.Bus, "40\n")]
        [InlineData(VehicleType.Boat, "12.5\n")]
        [InlineData(VehicleType.Truck, "9000\n")]
        public void VehicleTypes_CreateBuildsVehicleOfMatchingType(VehicleType type, string detailInput)
        {
            // Arrange
            using var output = new StringWriter();
            using var input = new StringReader(detailInput);
            var ui = new ConsoleUI(input, output, new GarageHandler(new InMemoryGarageStorage()));
            var info = ui.VehicleTypes.Single(i => i.Type == type);

            // Act
            Vehicle vehicle = info.Create("ABC123", "Black", 4, ParkingSizeLimits.For(type).Min);

            // Assert
            Assert.Equal(type, vehicle.Type);
        }
    }
}
