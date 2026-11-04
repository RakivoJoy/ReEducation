using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup;

/// <summary>
/// Default <see cref="IHandler"/>. Owns the garage and forwards each menu action to it.
/// Every method except <see cref="CreateGarage(int)"/> and <see cref="HasGarage"/> throws <see cref="InvalidOperationException"/> when no garage has been created.
/// </summary>
public class GarageHandler : IHandler
{
    // The garage is null until one is created (menu option 1).
    private IGarage<IVehicle>? _garage;
    private readonly IGarageStorage _storage;
    private readonly ISampleDataProvider _sampleData;

    /// <summary>
    /// Creates a handler that saves and loads the garage through the given storage and populates it from the given sample data.
    /// </summary>
    /// <param name="storage">Storage used for menu options 9 and 10.</param>
    /// <param name="sampleData">Source of the vehicles used for menu option 2.</param>
    public GarageHandler(IGarageStorage storage, ISampleDataProvider sampleData)
    {
        ArgumentNullException.ThrowIfNull(storage);
        ArgumentNullException.ThrowIfNull(sampleData);

        _storage = storage;
        _sampleData = sampleData;
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
    public OperationResult PopulateGarage() => ToOperationResult(RequireGarage().Populate(_sampleData.CreateSampleVehicles()));

    /// <inheritdoc/>
    public OperationResult ParkVehicle(IVehicle vehicle) => ToOperationResult(RequireGarage().AddVehicle(vehicle), vehicle.RegistrationNumber);

    /// <inheritdoc/>
    public OperationResult RemoveVehicle(string registrationNumber) => ToOperationResult(RequireGarage().RemoveVehicleByRegistration(registrationNumber));

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

    private IGarage<IVehicle> RequireGarage()
    {
        return _garage ?? throw new InvalidOperationException("No garage has been created yet.");
    }

    private static OperationResult ToOperationResult(ParkResult result, string registrationNumber) =>
        result.Parked
            ? OperationResult.Ok($"Vehicle {registrationNumber} parked.")
            : OperationResult.Fail(ParkFailureMessage(result.Reason!.Value, registrationNumber));

    private static OperationResult ToOperationResult(RemoveResult result) =>
        result.Removed
            ? OperationResult.Ok($"Vehicle {result.RegistrationNumber} removed.")
            : OperationResult.Fail($"No vehicle with registration number {result.RegistrationNumber} was found.");

    private static OperationResult ToOperationResult(PopulateResult result)
    {
        string message = $"Parked {result.Parked} of {result.Total} vehicles.";
        if (result.Failures.Count == 0)
        {
            return OperationResult.Ok(message);
        }

        IEnumerable<string> lines = result.Failures.Select(failure => $"{failure.RegistrationNumber}: {ParkFailureMessage(failure.Reason, failure.RegistrationNumber)}");
        return OperationResult.Fail(message + Environment.NewLine + string.Join(Environment.NewLine, lines));
    }

    private static string ParkFailureMessage(ParkFailureReason reason, string registrationNumber) =>
        reason == ParkFailureReason.Duplicate
            ? $"A vehicle with registration number {registrationNumber} is already parked."
            : "The garage is full.";
}
