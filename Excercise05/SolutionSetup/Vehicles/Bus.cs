namespace SolutionSetup.Vehicles
{
    public class Bus : Vehicle
    {
        public int NumberOfSeats { get; set; }

        public override VehicleType Type => VehicleType.Bus;

        public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
            : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfSeats = numberOfSeats;
        }
    }
}
