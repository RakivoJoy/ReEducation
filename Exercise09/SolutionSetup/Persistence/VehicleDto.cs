using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Serializable form of a vehicle. <see cref="Type"/> selects which type-specific fields are used.
/// </summary>
public sealed record VehicleDto(
    VehicleType Type,
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    FuelType? FuelType = null,
    int? NumberOfSeats = null,
    int? NumberOfEngines = null,
    int? LoadCapacity = null,
    int? CylinderVolume = null,
    double? Length = null);
