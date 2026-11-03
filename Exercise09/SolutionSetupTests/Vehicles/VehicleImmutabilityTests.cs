using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class VehicleImmutabilityTests
    {
        [Theory]
        [InlineData(typeof(Vehicle), nameof(Vehicle.Color))]
        [InlineData(typeof(Vehicle), nameof(Vehicle.NumberOfWheels))]
        [InlineData(typeof(Car), nameof(Car.FuelType))]
        [InlineData(typeof(Motorcycle), nameof(Motorcycle.CylinderVolume))]
        [InlineData(typeof(Airplane), nameof(Airplane.NumberOfEngines))]
        [InlineData(typeof(Bus), nameof(Bus.NumberOfSeats))]
        [InlineData(typeof(Boat), nameof(Boat.Length))]
        [InlineData(typeof(Truck), nameof(Truck.LoadCapacity))]
        public void Property_HasNoPublicSetter(Type vehicleType, string propertyName)
        {
            // Arrange
            var property = vehicleType.GetProperty(propertyName);

            // Act
            var setter = property!.SetMethod;

            // Assert
            Assert.Null(setter);
        }
    }
}

