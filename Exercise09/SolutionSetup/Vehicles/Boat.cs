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

    /// <summary>Adds the length to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Length: {Length.ToString(CultureInfo.InvariantCulture)} m";

    /// <summary>Creates a boat. Throws if the length is outside the allowed range.</summary>
    public Boat(string registrationNumber, string color, int numberOfWheels, double length, int? parkingSize = null)
        : base(VehicleType.Boat, registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(length, MinLength, MaxLength, "Length", "m", nameof(length));

        Length = length;
    }
}
