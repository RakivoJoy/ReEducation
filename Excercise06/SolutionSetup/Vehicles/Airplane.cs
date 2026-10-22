namespace SolutionSetup.Vehicles;

public class Airplane : Vehicle
{
    public int NumberOfEngines { get; set; }

    public override VehicleType Type => VehicleType.Airplane;

    protected override string Details => $"Engines: {NumberOfEngines}";

    public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines)
        : base(registrationNumber, color, numberOfWheels)
    {
        NumberOfEngines = numberOfEngines;
    }
}
