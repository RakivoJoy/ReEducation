namespace SolutionSetup.Vehicles
{
    public class Car : Vehicle
    {
        public FuelType FuelType { get; set; }

        public override VehicleType Type => VehicleType.Car;

        protected override string Details => $"Fuel: {FuelType}";

        public Car(string registrationNumber, string color, int numberOfWheels, FuelType fuelType)
            : base(registrationNumber, color, numberOfWheels)
        {
            FuelType = fuelType;
        }
    }
}
