namespace SolutionSetup.Vehicles
{
    public class Bus : Vehicle
    {
        public int NumberOfSeats { get; set; }

        public override VehicleType Type => VehicleType.Bus;

        protected override string Details => $"Seats: {NumberOfSeats}";

        public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
            : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfSeats = numberOfSeats;
        }
    }
}
