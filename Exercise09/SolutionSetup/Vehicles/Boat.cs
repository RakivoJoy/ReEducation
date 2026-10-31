using System.Globalization;

namespace SolutionSetup.Vehicles;

public class Boat : Vehicle
{
    public double Length { get; set; }

    public override VehicleType Type => VehicleType.Boat;

    protected override string Details => $"Length: {Length.ToString(CultureInfo.InvariantCulture)} m";

    public Boat(string registrationNumber, string color, int numberOfWheels, double length)
        : base(registrationNumber, color, numberOfWheels)
    {
        Length = length;
    }
}
