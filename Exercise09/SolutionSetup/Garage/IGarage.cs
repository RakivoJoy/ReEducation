using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Operations that consumers of a garage (handler, persistence) rely on.
/// </summary>
public interface IGarage<T> : IEnumerable<T> where T : IVehicle
{
    /// <summary>
    /// The total number of parking spaces, free or occupied.
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// Parks a vehicle in the garage.
    /// </summary>
    ParkResult AddVehicle(T vehicle);

    /// <summary>
    /// Removes the vehicle with the given registration number and frees its space.
    /// </summary>
    RemoveResult RemoveVehicleByRegistration(string registrationNumber);

    /// <summary>
    /// Tries to park every vehicle in the list and reports the outcome.
    /// </summary>
    PopulateResult Populate(IEnumerable<T> vehicles);

    /// <summary>
    /// Returns a read-only snapshot of the parked vehicles.
    /// </summary>
    IReadOnlyList<T> ListAllVehicles();

    /// <summary>
    /// Counts the parked vehicles per type.
    /// </summary>
    Dictionary<VehicleType, int> ListVehicleCountByType();

    /// <summary>
    /// Finds a parked vehicle by registration number, or null if none matches.
    /// </summary>
    T? FindByRegistration(string registrationNumber);

    /// <summary>
    /// Returns the parked vehicles that match the search criteria.
    /// </summary>
    IReadOnlyList<T> SearchByProperties(SearchCriteria criteria);
}
