namespace SolutionSetup.Vehicles;

public class Airplane : Vehicle
{
    /// <summary>Smallest allowed number of engines.</summary>
    public const int MinEngines = 1;

    /// <summary>Largest allowed number of engines.</summary>
    public const int MaxEngines = 8;

    /// <summary>Number of engines. Set once at construction.</summary>
    public int NumberOfEngines { get; }

    /// <summary>Always <see cref="VehicleType.Airplane"/>.</summary>
    public override VehicleType Type => VehicleType.Airplane;

    /// <summary>Adds the number of engines to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Engines: {NumberOfEngines}";

    /// <summary>Creates an airplane. Throws if the number of engines is outside the allowed range.</summary>
    public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(numberOfEngines, MinEngines, MaxEngines, "Number of engines", string.Empty, nameof(numberOfEngines));

        NumberOfEngines = numberOfEngines;
    }
}
