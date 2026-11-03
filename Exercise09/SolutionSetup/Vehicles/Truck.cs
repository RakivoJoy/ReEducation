using System;

namespace SolutionSetup.Vehicles;

public class Truck : Vehicle
{
    /// <summary>Smallest allowed load capacity in kg.</summary>
    public const int MinLoadCapacity = 1;

    /// <summary>Largest allowed load capacity in kg.</summary>
    public const int MaxLoadCapacity = 50000;

    /// <summary>Load capacity in kg. Set once at construction.</summary>
    public int LoadCapacity { get; }

    /// <summary>Always <see cref="VehicleType.Truck"/>.</summary>
    public override VehicleType Type => VehicleType.Truck;

    /// <summary>Adds the load capacity to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Load: {LoadCapacity} kg";

    /// <summary>Creates a truck. Throws if the load capacity is outside the allowed range.</summary>
    public Truck(string registrationNumber, string color, int numberOfWheels, int loadCapacity, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(loadCapacity, MinLoadCapacity, MaxLoadCapacity, "Load capacity", "kg", nameof(loadCapacity));

        LoadCapacity = loadCapacity;
    }
}
