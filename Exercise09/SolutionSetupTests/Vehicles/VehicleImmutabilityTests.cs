using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    // TODO: Phase 1 - Remove the Skip arguments once the vehicle properties are immutable after construction.
    public class VehicleImmutabilityTests
    {
        [Theory(Skip = "Phase 1: properties are still publicly settable.")]
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
            Assert.True(setter is null || !setter.IsPublic || setter.ReturnParameter.GetRequiredCustomModifiers().Length > 0);
        }
    }
}
