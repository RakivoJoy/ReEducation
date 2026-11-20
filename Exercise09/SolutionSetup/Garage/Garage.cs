using SolutionSetup.Vehicles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup;

/// <summary>
/// A garage that parks vehicles in a fixed number of spaces. Parked vehicles are stored in a list
/// whose size never exceeds the capacity. Registration numbers are unique and compared ignoring case.
/// </summary>
public class Garage<T> : IGarage<T> where T : IVehicle
{
    /// <summary>
    /// The largest capacity a garage can be created with.
    /// </summary>
    public static readonly int MaxCapacity = 2001;

    private readonly List<T> _vehicles = [];

    /// <summary>
    /// The total number of parking spaces, free or occupied.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Creates an empty garage with the given number of parking spaces.
    /// </summary>
    /// <param name="capacity">Number of parking spaces. Must be between 1 and <see cref="MaxCapacity"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is outside the allowed range.</exception>
    public Garage(int capacity)
    {
        if (capacity <= 0 || capacity > MaxCapacity)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), $"Capacity must be between 1 and {MaxCapacity}.");
        }

        Capacity = capacity;
    }

    /// <summary>
    /// Parks a vehicle in the garage.
    /// </summary>
    /// <returns>A failed result with the reason if the registration number is already parked or the garage is full; otherwise a success result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is null.</exception>
    public ParkResult AddVehicle(T vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (FindByRegistration(vehicle.RegistrationNumber) is not null)
        {
            return ParkResult.Failure(ParkFailureReason.Duplicate);
        }

        if (_vehicles.Count >= Capacity)
        {
            return ParkResult.Failure(ParkFailureReason.Full);
        }

        _vehicles.Add(vehicle);
        return ParkResult.Success();
    }

    /// <summary>
    /// Removes the vehicle with the given registration number (case-insensitive) and frees its space.
    /// </summary>
    /// <returns>A result that reports whether a vehicle was removed.</returns>
    public RemoveResult RemoveVehicleByRegistration(string registrationNumber)
    {
        T? vehicle = FindByRegistration(registrationNumber);
        if (vehicle is null)
        {
            return new RemoveResult(false, registrationNumber);
        }

        _vehicles.Remove(vehicle);
        return new RemoveResult(true, registrationNumber);
    }

    /// <summary>
    /// Tries to park every vehicle in the list. Vehicles that cannot be parked (duplicate or no space) are skipped.
    /// </summary>
    /// <returns>The number of vehicles attempted and parked, and the failures with their reasons.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicles"/> is null.</exception>
    public PopulateResult Populate(IEnumerable<T> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles);

        int total = 0;
        int parked = 0;
        List<(string RegistrationNumber, ParkFailureReason Reason)> failures = [];

        foreach (T vehicle in vehicles)
        {
            total++;
            ParkResult result = AddVehicle(vehicle);
            if (result.Parked)
            {
                parked++;
            }
            else
            {
                failures.Add((vehicle.RegistrationNumber, result.Reason!.Value));
            }
        }

        return new PopulateResult(total, parked, failures);
    }

    /// <summary>
    /// Returns a read-only snapshot of the parked vehicles.
    /// </summary>
    public IReadOnlyList<T> ListAllVehicles()
    {
        return new List<T>(_vehicles).AsReadOnly();
    }

    /// <summary>
    /// Counts the parked vehicles per type. Types with no parked vehicles are absent from the result.
    /// </summary>
    public Dictionary<VehicleType, int> ListVehicleCountByType()
    {
        return _vehicles.GroupBy(v => v.Type).ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Finds a parked vehicle by registration number, ignoring case and surrounding spaces.
    /// </summary>
    /// <returns>The vehicle, or null if none matches.</returns>
    public T? FindByRegistration(string registrationNumber)
    {
        string? trimmed = registrationNumber?.Trim();
        return _vehicles.FirstOrDefault(v => string.Equals(v.RegistrationNumber, trimmed, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns the parked vehicles that match every filter set in the criteria. Filters left null are ignored.
    /// Text filters are compared ignoring case.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is null.</exception>
    public IReadOnlyList<T> SearchByProperties(SearchCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        return _vehicles.Where(v => criteria.Matches(v)).ToList();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _vehicles.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
