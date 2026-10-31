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

    public override VehicleType Type => VehicleType.Truck;

    protected override string Details => $"Load: {LoadCapacity} kg";

    public Truck(string registrationNumber, string color, int numberOfWheels, int loadCapacity)
        : base(registrationNumber, color, numberOfWheels)
    {
        ThrowIfOutOfRange(loadCapacity, MinLoadCapacity, MaxLoadCapacity, "Load capacity", "kg", nameof(loadCapacity));

        LoadCapacity = loadCapacity;
    }
}
