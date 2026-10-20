namespace SolutionSetup.Vehicles
{
    public class Car : Vehicle
    {
        public FuelType FuelType { get; set; }

        public override VehicleType Type => VehicleType.Car;

        public Car(string registrationNumber, string color, int numberOfWheels, FuelType fuelType)
            : base(registrationNumber, color, numberOfWheels)
        {
            FuelType = fuelType;
        }
    }
}
