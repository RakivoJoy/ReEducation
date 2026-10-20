namespace SolutionSetup.Vehicles
{
    public abstract class Vehicle
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int NumberOfWheels { get; set; }
        public abstract VehicleType Type { get; }

        protected Vehicle(string registrationNumber, string color, int numberOfWheels)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            NumberOfWheels = numberOfWheels;
        }

        public override string ToString() => GetType().Name;
    }
}
