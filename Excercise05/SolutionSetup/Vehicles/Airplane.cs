namespace SolutionSetup.Vehicles
{
    public class Airplane : Vehicle
    {
        public int NumberOfEngines { get; set; }

        public override VehicleType Type => VehicleType.Airplane;

        public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines)
            : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfEngines = numberOfEngines;
        }
    }
}
