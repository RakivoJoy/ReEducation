using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Outcome of loading a garage: the loaded garage on success, or null with the reason on failure.
/// </summary>
public sealed record GarageLoadResult(bool Success, string Message, IGarage<IVehicle>? Garage)
{
    /// <summary>
    /// Creates a successful result holding the loaded garage.
    /// </summary>
    public static GarageLoadResult Ok(IGarage<IVehicle> garage, string message) => new(true, message, garage);

    /// <summary>
    /// Creates a failed result with the given reason.
    /// </summary>
    public static GarageLoadResult Fail(string message) => new(false, message, null);
}
