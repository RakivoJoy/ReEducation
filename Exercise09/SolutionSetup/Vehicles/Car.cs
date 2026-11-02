namespace SolutionSetup.Vehicles;

public class Car : Vehicle
{
    public FuelType FuelType { get; }

    public override VehicleType Type => VehicleType.Car;

    protected override string Details => $"Fuel: {FuelType}";

    public Car(string registrationNumber, string color, int numberOfWheels, FuelType fuelType, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        FuelType = fuelType;
    }
}
