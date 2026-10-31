namespace SolutionSetup.Vehicles;

public class Airplane : Vehicle
{
    /// <summary>Smallest allowed number of engines.</summary>
    public const int MinEngines = 1;

    /// <summary>Largest allowed number of engines.</summary>
    public const int MaxEngines = 8;

    /// <summary>Number of engines. Set once at construction.</summary>
    public int NumberOfEngines { get; }

    public override VehicleType Type => VehicleType.Airplane;

    protected override string Details => $"Engines: {NumberOfEngines}";

    public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines)
        : base(registrationNumber, color, numberOfWheels)
    {
        ThrowIfOutOfRange(numberOfEngines, MinEngines, MaxEngines, "Number of engines", string.Empty, nameof(numberOfEngines));

        NumberOfEngines = numberOfEngines;
    }
}
