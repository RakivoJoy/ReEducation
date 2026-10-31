using System;

namespace SolutionSetup.Vehicles;

public class Truck : Vehicle
{
    // TODO: Phase 1 - Make LoadCapacity immutable after construction (private/init set).
    public int LoadCapacity { get; set; }

    public override VehicleType Type => VehicleType.Truck;

    protected override string Details => $"Load: {LoadCapacity} kg";

    public Truck(string registrationNumber, string color, int numberOfWheels, int loadCapacity)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate a range for load capacity (kg), not only "not negative", with a clear message including the range and unit.
        ArgumentOutOfRangeException.ThrowIfNegative(loadCapacity);

        LoadCapacity = loadCapacity;
    }
}
