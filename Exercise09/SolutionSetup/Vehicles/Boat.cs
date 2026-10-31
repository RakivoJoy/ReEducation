using System.Globalization;

namespace SolutionSetup.Vehicles;

public class Boat : Vehicle
{
    /// <summary>Shortest allowed length in meters.</summary>
    public const double MinLength = 1;

    /// <summary>Longest allowed length in meters.</summary>
    public const double MaxLength = 400;

    /// <summary>Length in meters. Set once at construction.</summary>
    public double Length { get; }

    public override VehicleType Type => VehicleType.Boat;

    protected override string Details => $"Length: {Length.ToString(CultureInfo.InvariantCulture)} m";

    public Boat(string registrationNumber, string color, int numberOfWheels, double length)
        : base(registrationNumber, color, numberOfWheels)
    {
        ThrowIfOutOfRange(length, MinLength, MaxLength, "Length", "m", nameof(length));

        Length = length;
    }
}
