using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetup
{
    /// <summary>
    /// Ready-made vehicles used to populate a garage (menu option 2).
    /// </summary>
    public static class SampleData
    {
        /// <summary>
        /// Creates three cars with unique registration numbers.
        /// </summary>
        public static List<Vehicle> CreateSampleVehicles() =>
        [
            new Car("ABC123", "Black", 4, FuelType.Gasoline),
            new Car("DEF456", "Red", 4, FuelType.Diesel),
            new Car("GHI789", "White", 4, FuelType.Gasoline)
        ];
    }
}
