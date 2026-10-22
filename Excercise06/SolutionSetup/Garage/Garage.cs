using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// A garage that parks vehicles in a fixed number of spaces. Parked vehicles are stored in a list
/// whose size never exceeds the capacity. Registration numbers are unique and compared ignoring case.
/// </summary>
public class Garage
{
    private readonly List<Vehicle> _vehicles = new List<Vehicle>();

    /// <summary>
    /// The total number of parking spaces, free or occupied.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Creates an empty garage with the given number of parking spaces.
    /// </summary>
    /// <param name="capacity">Number of parking spaces. Must be greater than zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is zero or negative.</exception>
    public Garage(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        Capacity = capacity;
    }

    /// <summary>
    /// Parks a vehicle in the garage.
    /// </summary>
    /// <returns>A failed result if the registration number is already parked or the garage is full; otherwise a success result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicle"/> is null.</exception>
    public OperationResult AddVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (FindByRegistration(vehicle.RegistrationNumber) is not null)
        {
            return OperationResult.Fail($"A vehicle with registration number {vehicle.RegistrationNumber} is already parked.");
        }

        if (_vehicles.Count >= Capacity)
        {
            return OperationResult.Fail("The garage is full.");
        }

        _vehicles.Add(vehicle);
        return OperationResult.Ok($"Vehicle {vehicle.RegistrationNumber} parked.");
    }

    /// <summary>
    /// Removes the vehicle with the given registration number (case-insensitive) and frees its space.
    /// </summary>
    /// <returns>A failed result if no such vehicle is parked; otherwise a success result.</returns>
    public OperationResult RemoveVehicleByRegistration(string registrationNumber)
    {
        int index = FindIndex(registrationNumber);
        if (index < 0)
        {
            return OperationResult.Fail($"No vehicle with registration number {registrationNumber} was found.");
        }

        _vehicles.RemoveAt(index);
        return OperationResult.Ok($"Vehicle {registrationNumber} removed.");
    }

    /// <summary>
    /// Tries to park every vehicle in the list. Vehicles that cannot be parked (duplicate or no space) are skipped.
    /// </summary>
    /// <returns>A success result if all vehicles were parked; otherwise a failed result. The message states how many were parked.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="vehicles"/> is null.</exception>
    public OperationResult Populate(List<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles);

        int parked = 0;
        for (int i = 0; i < vehicles.Count; i++)
        {
            if (AddVehicle(vehicles[i]).Success)
            {
                parked++;
            }
        }

        string message = $"Parked {parked} of {vehicles.Count} vehicles.";
        return parked == vehicles.Count ? OperationResult.Ok(message) : OperationResult.Fail(message);
    }

    /// <summary>
    /// Returns a new list of the parked vehicles.
    /// </summary>
    public List<Vehicle> ListAllVehicles()
    {
        return new List<Vehicle>(_vehicles);
    }

    /// <summary>
    /// Counts the parked vehicles per type. The returned array is indexed by (int)VehicleType.
    /// </summary>
    public int[] ListVehicleCountByType()
    {
        int[] counts = new int[Enum.GetNames(typeof(VehicleType)).Length];
        for (int i = 0; i < _vehicles.Count; i++)
        {
            counts[(int)_vehicles[i].Type]++;
        }

        return counts;
    }

    /// <summary>
    /// Finds a parked vehicle by registration number, ignoring case and surrounding spaces.
    /// </summary>
    /// <returns>The vehicle, or null if none matches.</returns>
    public Vehicle? FindByRegistration(string registrationNumber)
    {
        int index = FindIndex(registrationNumber);
        return index < 0 ? null : _vehicles[index];
    }

    /// <summary>
    /// Returns the parked vehicles that match every filter set in the criteria. Filters left null are ignored.
    /// Text filters are compared ignoring case.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="criteria"/> is null.</exception>
    public List<Vehicle> SearchByProperties(SearchCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        List<Vehicle> matches = new List<Vehicle>();
        for (int i = 0; i < _vehicles.Count; i++)
        {
            Vehicle vehicle = _vehicles[i];

            bool matchesRegistration = criteria.RegistrationNumber is null
                || string.Equals(vehicle.RegistrationNumber, criteria.RegistrationNumber, StringComparison.OrdinalIgnoreCase);
            bool matchesColor = criteria.Color is null
                || string.Equals(vehicle.Color, criteria.Color, StringComparison.OrdinalIgnoreCase);
            bool matchesWheels = criteria.NumberOfWheels is null
                || vehicle.NumberOfWheels == criteria.NumberOfWheels;
            bool matchesType = criteria.Type is null
                || vehicle.Type == criteria.Type;

            if (matchesRegistration && matchesColor && matchesWheels && matchesType)
            {
                matches.Add(vehicle);
            }
        }

        return matches;
    }

    // Returns the index of the vehicle with the given registration number (case-insensitive, trimmed), or -1.
    private int FindIndex(string registrationNumber)
    {
        string? trimmed = registrationNumber?.Trim();
        for (int i = 0; i < _vehicles.Count; i++)
        {
            if (string.Equals(_vehicles[i].RegistrationNumber, trimmed, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }
}
