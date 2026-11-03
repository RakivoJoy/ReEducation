namespace SolutionSetup.Vehicles;

public class Bus : Vehicle
{
    /// <summary>Smallest allowed number of seats.</summary>
    public const int MinSeats = 1;

    /// <summary>Largest allowed number of seats.</summary>
    public const int MaxSeats = 150;

    /// <summary>Number of seats. Set once at construction.</summary>
    public int NumberOfSeats { get; }

    /// <summary>Always <see cref="VehicleType.Bus"/>.</summary>
    public override VehicleType Type => VehicleType.Bus;

    /// <summary>Adds the number of seats to the description shown by <see cref="Vehicle.ToString"/>.</summary>
    protected override string Details => $"Seats: {NumberOfSeats}";

    /// <summary>Creates a bus. Throws if the number of seats is outside the allowed range.</summary>
    public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats, int? parkingSize = null)
        : base(registrationNumber, color, numberOfWheels, parkingSize)
    {
        ThrowIfOutOfRange(numberOfSeats, MinSeats, MaxSeats, "Number of seats", string.Empty, nameof(numberOfSeats));

        NumberOfSeats = numberOfSeats;
    }
}
