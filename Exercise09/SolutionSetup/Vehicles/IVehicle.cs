namespace SolutionSetup.Vehicles;

/// <summary>
/// Common contract for all vehicles that can be parked in a garage.
/// </summary>
public interface IVehicle
{
    /// <summary>Unique registration number, stored trimmed and uppercase.</summary>
    string RegistrationNumber { get; }

    /// <summary>Color of the vehicle.</summary>
    string Color { get; }

    /// <summary>Number of wheels on the vehicle.</summary>
    int NumberOfWheels { get; }

    /// <summary>The vehicle type.</summary>
    VehicleType Type { get; }
}
