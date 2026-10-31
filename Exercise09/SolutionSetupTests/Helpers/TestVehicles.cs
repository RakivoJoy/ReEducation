using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    internal static class TestVehicles
    {
        public static Car CreateCar(string registrationNumber, string color = "Black", FuelType fuelType = FuelType.Gasoline) =>
            new(registrationNumber, color, 4, fuelType);
    }
}
