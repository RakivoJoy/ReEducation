using System.Threading;
using System.Threading.Tasks;
using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Saves and loads a garage. Expected failures are returned as results rather than thrown.
/// </summary>
public interface IGarageStorage
{
    /// <summary>
    /// Saves the garage, replacing any previous save.
    /// </summary>
    Task<OperationResult> SaveAsync(Garage<IVehicle> garage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads the saved garage.
    /// </summary>
    Task<GarageLoadResult> LoadAsync(CancellationToken cancellationToken = default);
}
