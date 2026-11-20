using System.Collections.Generic;

namespace SolutionSetup.Vehicles;

/// <summary>
/// Describes a vehicle to be created: its type, common properties and type-specific details.
/// The garage layer passes it to <see cref="VehicleFactory"/>, so callers never construct vehicles directly.
/// </summary>
public sealed record VehicleRequest(
    VehicleType Type,
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    IReadOnlyDictionary<string, object> Details);
