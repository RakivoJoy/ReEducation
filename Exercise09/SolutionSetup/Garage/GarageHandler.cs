using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Default <see cref="IHandler"/>. Owns the garage and forwards each menu action to it.
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
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public OperationResult PopulateGarage() => RequireGarage().Populate(SampleData.CreateSampleVehicles());

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public OperationResult ParkVehicle(IVehicle vehicle) => RequireGarage().AddVehicle(vehicle);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public OperationResult RemoveVehicle(string registrationNumber) => RequireGarage().RemoveVehicleByRegistration(registrationNumber);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public IReadOnlyList<IVehicle> ListVehicles() => RequireGarage().ListAllVehicles();

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public Dictionary<VehicleType, int> ListVehicleCounts() => RequireGarage().ListVehicleCountByType();

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public IVehicle? FindVehicle(string registrationNumber) => RequireGarage().FindByRegistration(registrationNumber);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no garage has been created.</exception>
    public IReadOnlyList<IVehicle> SearchVehicles(SearchCriteria criteria) => RequireGarage().SearchByProperties(criteria);

    private Garage<IVehicle> RequireGarage()
    {
        return _garage ?? throw new InvalidOperationException("No garage has been created yet.");
    }
}
