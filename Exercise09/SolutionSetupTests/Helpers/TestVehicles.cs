using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetupTests
{
    internal static class TestVehicles
    {
        public static Car CreateCar(string registrationNumber, string color = "Black", FuelType fuelType = FuelType.Gasoline) =>
            new(registrationNumber, color, 4, fuelType);

        public static VehicleRequest CarRequest(string registrationNumber, string color = "Black", FuelType fuelType = FuelType.Gasoline) =>
            new(VehicleType.Car, registrationNumber, color, 4, 5, new Dictionary<string, object> { [VehicleFactory.FuelTypeKey] = fuelType });

        public static VehicleRequest TruckRequest(string registrationNumber, string color, int numberOfWheels, int loadCapacity) =>
            new(VehicleType.Truck, registrationNumber, color, numberOfWheels, 15, new Dictionary<string, object> { [VehicleFactory.LoadCapacityKey] = loadCapacity });
    }
}
