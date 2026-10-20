namespace SolutionSetup.Vehicles
{
    public class Boat : Vehicle
    {
        public double Length { get; set; }

        public override VehicleType Type => VehicleType.Boat;

        public Boat(string registrationNumber, string color, int numberOfWheels, double length)
            : base(registrationNumber, color, numberOfWheels)
        {
            Length = length;
        }
    }
}
