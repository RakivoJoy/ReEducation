namespace SolutionSetup.Vehicles
{
    public class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; set; }

        public override VehicleType Type => VehicleType.Motorcycle;

        public Motorcycle(string registrationNumber, string color, int numberOfWheels, int cylinderVolume)
            : base(registrationNumber, color, numberOfWheels)
        {
            CylinderVolume = cylinderVolume;
        }
    }
}
