namespace SolutionSetup.Vehicles;

public class Motorcycle : Vehicle
{
    /// <summary>Smallest allowed cylinder volume in cc.</summary>
    public const int MinCylinderVolume = 50;

    /// <summary>Largest allowed cylinder volume in cc.</summary>
    public const int MaxCylinderVolume = 3000;

    /// <summary>Cylinder volume in cc. Set once at construction.</summary>
    public int CylinderVolume { get; }

    public override VehicleType Type => VehicleType.Motorcycle;

    protected override string Details => $"Cylinder volume: {CylinderVolume} cc";

    public Motorcycle(string registrationNumber, string color, int numberOfWheels, int cylinderVolume, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(cylinderVolume, MinCylinderVolume, MaxCylinderVolume, "Cylinder volume", "cc", nameof(cylinderVolume));

        CylinderVolume = cylinderVolume;
    }
}
