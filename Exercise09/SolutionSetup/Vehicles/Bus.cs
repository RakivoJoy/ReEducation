namespace SolutionSetup.Vehicles;

public class Bus : Vehicle
{
    // TODO: Phase 1 - Make NumberOfSeats immutable after construction (private/init set).
    public int NumberOfSeats { get; set; }

    public override VehicleType Type => VehicleType.Bus;

    protected override string Details => $"Seats: {NumberOfSeats}";

    public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate the number of seats with a range and a clear error message.
        NumberOfSeats = numberOfSeats;
    }
}
