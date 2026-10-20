using SolutionSetup.Vehicles;

namespace SolutionSetup
{
    public static class SampleData
    {
        public static Vehicle[] CreateSampleVehicles() =>
        [
            new Car("ABC123", "Black", 4, FuelType.Gasoline),
            new Car("DEF456", "Red", 4, FuelType.Diesel),
            new Car("GHI789", "White", 4, FuelType.Gasoline)
        ];
    }
}
