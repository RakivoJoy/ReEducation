using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Default <see cref="IHandler"/>. Owns the garage and forwards each menu action to it.
/// Every method except <see cref="CreateGarage(int)"/> and <see cref="HasGarage"/> throws <see cref="InvalidOperationException"/> when no garage has been created.
/// </summary>
public class GarageHandler : IHandler
{
    // The garage is null until one is created (menu option 1).
    private Garage<IVehicle>? _garage;

    /// <inheritdoc/>
    public bool HasGarage => _garage is not null;

    /// <inheritdoc/>
    public OperationResult CreateGarage(int capacity)
    {
        try
        {
            _garage = new Garage<IVehicle>(capacity);
            return OperationResult.Ok($"Garage created with capacity {capacity}.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return OperationResult.Fail($"Could not create garage: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public OperationResult PopulateGarage()

    /// <inheritdoc/>
    public OperationResult ParkVehicle(

    /// <inheritdoc/>
    public OperationResult RemoveVehicle(

    /// <inheritdoc/>
    public IReadOnlyList<IVehicle> ListVehicles()

    /// <inheritdoc/>
    public Dictionary<VehicleType, int> ListVehicleCounts()

    /// <inheritdoc/>
    public IVehicle? FindVehicle(

    /// <inheritdoc/>
    public IReadOnlyList<IVehicle> SearchVehicles(

    private Garage<IVehicle> RequireGarage()
    {
        return _garage ?? throw new InvalidOperationException("No garage has been created yet.");
    }
}
