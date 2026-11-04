using System;
using System.Collections.Generic;

namespace SolutionSetup.Vehicles;

/// <summary>
/// Builds a vehicle of any <see cref="VehicleType"/> from its common properties and type-specific details,
/// so callers do not need to know which concrete constructor to use.
/// </summary>
public static class VehicleFactory
{
    /// <summary>Type-specific detail keys: cylinder volume, number of engines, fuel type, number of seats, length and load capacity.</summary>
    public const string CylinderVolumeKey = "cylinderVolume";
    public const string NumberOfEnginesKey = "numberOfEngines";
    public const string FuelTypeKey = "fuelType";
    public const string NumberOfSeatsKey = "numberOfSeats";
    public const string LengthKey = "length";
    public const string LoadCapacityKey = "loadCapacity";

    /// <summary>Creates a vehicle of the given type. Throws if a required detail is missing or a value is invalid.</summary>
    public static Vehicle Create(
        VehicleType type,
        string registrationNumber,
        string color,
        int numberOfWheels,
        int parkingSize,
        IReadOnlyDictionary<string, object> details)
    {
        ArgumentNullException.ThrowIfNull(details);

        return type switch
        {
            VehicleType.Motorcycle => new Motorcycle(registrationNumber, color, numberOfWheels, GetInt(details, CylinderVolumeKey), parkingSize),
            VehicleType.Airplane => new Airplane(registrationNumber, color, numberOfWheels, GetInt(details, NumberOfEnginesKey), parkingSize),
            VehicleType.Car => new Car(registrationNumber, color, numberOfWheels, Get<FuelType>(details, FuelTypeKey), parkingSize),
            VehicleType.Bus => new Bus(registrationNumber, color, numberOfWheels, GetInt(details, NumberOfSeatsKey), parkingSize),
            VehicleType.Boat => new Boat(registrationNumber, color, numberOfWheels, Get<double>(details, LengthKey), parkingSize),
            VehicleType.Truck => new Truck(registrationNumber, color, numberOfWheels, GetInt(details, LoadCapacityKey), parkingSize),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported vehicle type."),
        };
    }

    private static int GetInt(IReadOnlyDictionary<string, object> details, string key) => Get<int>(details, key);

    private static T Get<T>(IReadOnlyDictionary<string, object> details, string key)
    {
        if (!details.TryGetValue(key, out object? value) || value is not T typed)
        {
            throw new ArgumentException($"Missing or invalid vehicle detail '{key}'.", nameof(details));
        }

        return typed;
    }
}
