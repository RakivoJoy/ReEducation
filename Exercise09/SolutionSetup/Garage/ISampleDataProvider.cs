using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Supplies the vehicles used to populate a garage (menu option 2).
/// </summary>
public interface ISampleDataProvider
{
    /// <summary>
    /// Creates the vehicles to add to a garage.
    /// </summary>
    IReadOnlyList<IVehicle> CreateSampleVehicles();
}
