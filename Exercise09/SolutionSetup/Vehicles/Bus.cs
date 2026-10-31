namespace SolutionSetup.Vehicles;

public class Bus : Vehicle
{
    /// <summary>Smallest allowed number of seats.</summary>
    public const int MinSeats = 1;

    /// <summary>Largest allowed number of seats.</summary>
    public const int MaxSeats = 150;

    /// <summary>Number of seats. Set once at construction.</summary>
    public int NumberOfSeats { get; }

    public override VehicleType Type => VehicleType.Bus;

    protected override string Details => $"Seats: {NumberOfSeats}";

    public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
        : base(registrationNumber, color, numberOfWheels)
    {
        ThrowIfOutOfRange(numberOfSeats, MinSeats, MaxSeats, "Number of seats", string.Empty, nameof(numberOfSeats));

        NumberOfSeats = numberOfSeats;
    }
}
