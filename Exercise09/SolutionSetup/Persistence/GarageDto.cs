using System.Collections.Generic;

namespace SolutionSetup.Persistence;

/// <summary>
/// Serializable form of a garage. Used by the JSON storage; not used by the domain classes.
/// </summary>
public sealed record GarageDto(int Version, int Capacity, List<VehicleDto> Vehicles);
