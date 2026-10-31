namespace SolutionSetup.Vehicles;

public class Airplane : Vehicle
{
    public int NumberOfEngines { get; }

    public override VehicleType Type => VehicleType.Airplane;

    protected override string Details => $"Engines: {NumberOfEngines}";

    public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate the number of engines (e.g. no negative values) with a clear error message.
        NumberOfEngines = numberOfEngines;
    }
}
