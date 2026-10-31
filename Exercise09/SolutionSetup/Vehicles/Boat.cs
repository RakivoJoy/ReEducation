using System.Globalization;

namespace SolutionSetup.Vehicles;

public class Boat : Vehicle
{
    public double Length { get; }

    public override VehicleType Type => VehicleType.Boat;

    protected override string Details => $"Length: {Length.ToString(CultureInfo.InvariantCulture)} m";

    public Boat(string registrationNumber, string color, int numberOfWheels, double length)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate length (m) with a lower and a sensible upper limit; it can currently be negative.
        Length = length;
    }
}
