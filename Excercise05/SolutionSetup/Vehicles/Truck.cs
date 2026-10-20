namespace SolutionSetup.Vehicles
{
    public class Truck : Vehicle
    {
        public int LoadCapacity { get; set; }

        public override VehicleType Type => VehicleType.Truck;

        protected override string Details => $"Load: {LoadCapacity} kg";

        public Truck(string registrationNumber, string color, int numberOfWheels, int loadCapacity)
            : base(registrationNumber, color, numberOfWheels)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(loadCapacity);

            LoadCapacity = loadCapacity;
        }
    }
}
