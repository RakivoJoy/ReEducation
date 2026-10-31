namespace SolutionSetup.Vehicles;

public class Motorcycle : Vehicle
{
    public int CylinderVolume { get; }

    public override VehicleType Type => VehicleType.Motorcycle;

    protected override string Details => $"Cylinder volume: {CylinderVolume} cc";

    public Motorcycle(string registrationNumber, string color, int numberOfWheels, int cylinderVolume)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate cylinder volume (cc) with a range and a clear error message.
        CylinderVolume = cylinderVolume;
    }
}
