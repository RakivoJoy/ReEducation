namespace SolutionSetup.Vehicles;

public class Motorcycle : Vehicle
{
    /// <summary>Smallest allowed cylinder volume in cc.</summary>
    public const int MinCylinderVolume = 50;

    /// <summary>Largest allowed cylinder volume in cc.</summary>
    public const int MaxCylinderVolume = 3000;

    /// <summary>Cylinder volume in cc. Set once at construction.</summary>
    public int CylinderVolume { get; }

    /// <summary>Always <see cref="VehicleType.Motorcycle"/>.</summary>
    public override VehicleType Type => VehicleType.Motorcycle;

    /// <summary>Adds the cylinder volume to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Cylinder volume: {CylinderVolume} cc";

    /// <summary>Creates a motorcycle. Throws if the cylinder volume is outside the allowed range.</summary>
    public Motorcycle(string registrationNumber, string color, int numberOfWheels, int cylinderVolume, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(cylinderVolume, MinCylinderVolume, MaxCylinderVolume, "Cylinder volume", "cc", nameof(cylinderVolume));

        CylinderVolume = cylinderVolume;
    }
}
