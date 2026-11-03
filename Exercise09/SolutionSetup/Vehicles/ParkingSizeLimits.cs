using System;

namespace SolutionSetup.Vehicles;

/// <summary>
/// The allowed parking size for each vehicle type. Every limit lies inside the overall range
/// <see cref="Min"/> to <see cref="Max"/>. Vehicle validation and the park-vehicle prompt both read this table,
/// so the limits are defined in one place.
/// </summary>
public static class ParkingSizeLimits
{
    /// <summary>Smallest parking size any vehicle can have.</summary>
    public const int Min = 1;

    /// <summary>Largest parking size any vehicle can have.</summary>
    public const int Max = 400;

    /// <summary>Returns the inclusive parking size range allowed for the vehicle type.</summary>
    public static (int Min, int Max) For(VehicleType type) => type switch
    {
        VehicleType.Motorcycle => (1, 10),
        VehicleType.Car => (5, 30),
        VehicleType.Bus => (20, 100),
        VehicleType.Truck => (15, 80),
        VehicleType.Boat => (10, Max),
        VehicleType.Airplane => (50, Max),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown vehicle type.")
    };
}
