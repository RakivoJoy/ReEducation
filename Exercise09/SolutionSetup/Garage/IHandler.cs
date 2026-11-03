using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Sits between a user interface and the garage. It holds the garage and exposes one method per menu option,
/// so a UI never touches the garage directly and can be tested without Console input or output.
/// </summary>
public interface IHandler
{
    /// <summary>True once a garage has been created.</summary>
    bool HasGarage { get; }

    /// <summary>Menu option 1. Creates a new garage with the given capacity, replacing any existing garage.</summary>
    OperationResult CreateGarage(int capacity);

    /// <summary>Menu option 2. Parks the sample vehicles in the garage.</summary>
    OperationResult PopulateGarage();

    /// <summary>Menu option 3. Parks the given vehicle.</summary>
    OperationResult ParkVehicle(IVehicle vehicle);

    /// <summary>Menu option 4. Removes the vehicle with the given registration number.</summary>
    OperationResult RemoveVehicle(string registrationNumber);

    /// <summary>Menu option 5. Returns a snapshot of all parked vehicles.</summary>
    IReadOnlyList<IVehicle> ListVehicles();

    /// <summary>Menu option 6. Returns the number of parked vehicles per type.</summary>
    Dictionary<VehicleType, int> ListVehicleCounts();

    /// <summary>Menu option 7. Returns the vehicle with the given registration number, or null.</summary>
    IVehicle? FindVehicle(string registrationNumber);

    /// <summary>Menu option 8. Returns the vehicles matching every filter set in the criteria.</summary>
    IReadOnlyList<IVehicle> SearchVehicles(SearchCriteria criteria);

    /// <summary>Menu option 9. Saves the current garage to storage.</summary>
    OperationResult SaveGarage();

    /// <summary>Menu option 10. Replaces the current garage with the one loaded from storage.</summary>
    OperationResult LoadGarage();
}
