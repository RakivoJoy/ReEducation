namespace SolutionSetup.Vehicles
{
    public class Truck : Vehicle
    {
        public int LoadCapacity { get; set; }

        public override VehicleType Type => VehicleType.Truck;

        public Truck(string registrationNumber, string color, int numberOfWheels, int loadCapacity)
            : base(registrationNumber, color, numberOfWheels)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(loadCapacity);

            LoadCapacity = loadCapacity;
        }
    }
}
