namespace SolutionSetup.Vehicles;

public class Car : Vehicle
{
    /// <summary>Type of fuel the car uses. Set once at construction.</summary>
    public FuelType FuelType { get; }

    /// <summary>Adds the fuel type to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Fuel: {FuelType}";

    /// <summary>Creates a car.</summary>
    public Car(string registrationNumber, string color, int numberOfWheels, FuelType fuelType, int? parkingSize = null)
        : base(VehicleType.Car, registrationNumber, color, numberOfWheels, parkingSize)
    {
        FuelType = fuelType;
    }
}
