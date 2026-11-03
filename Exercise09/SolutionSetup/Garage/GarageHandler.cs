using SolutionSetup.Persistence;
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
    private readonly IGarageStorage _storage;

    /// <summary>
    /// Creates a handler that saves and loads the garage at <see cref="JsonGarageStorage.DefaultFilePath"/>.
    /// </summary>
    public GarageHandler()
        : this(new JsonGarageStorage(JsonGarageStorage.DefaultFilePath))
    {
    }

    /// <summary>
    /// Creates a handler that saves and loads the garage through the given storage.
    /// </summary>
    /// <param name="storage">Storage used for menu options 9 and 10.</param>
    public GarageHandler(IGarageStorage storage)
    {
        ArgumentNullException.ThrowIfNull(storage);

        _storage = storage;
    }

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
    public OperationResult PopulateGarage() => RequireGarage().Populate(SampleData.CreateSampleVehicles());

    /// <inheritdoc/>
    public OperationResult ParkVehicle(IVehicle vehicle) => RequireGarage().AddVehicle(vehicle);

    /// <inheritdoc/>
    public OperationResult RemoveVehicle(string registrationNumber) => RequireGarage().RemoveVehicleByRegistration(registrationNumber);

    /// <inheritdoc/>
    public IReadOnlyList<IVehicle> ListVehicles() => RequireGarage().ListAllVehicles();

    /// <inheritdoc/>
    public Dictionary<VehicleType, int> ListVehicleCounts() => RequireGarage().ListVehicleCountByType();

    /// <inheritdoc/>
    public OperationResult SaveGarage() => _storage.SaveAsync(RequireGarage()).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public OperationResult LoadGarage()
    {
        GarageLoadResult result = _storage.LoadAsync().GetAwaiter().GetResult();
        if (result.Success && result.Garage is not null)
        {
            _garage = result.Garage;
            return OperationResult.Ok(result.Message);
        }

        return OperationResult.Fail(result.Message);
    }

    /// <inheritdoc/>
    public IVehicle? FindVehicle(string registrationNumber) => RequireGarage().FindByRegistration(registrationNumber);

    /// <inheritdoc/>
    public IReadOnlyList<IVehicle> SearchVehicles(SearchCriteria criteria) => RequireGarage().SearchByProperties(criteria);

    private Garage<IVehicle> RequireGarage()
    {
        return _garage ?? throw new InvalidOperationException("No garage has been created yet.");
    }
}
