namespace SolutionSetup.Vehicles;

public class Bus : Vehicle
{
    public int NumberOfSeats { get; }

    public override VehicleType Type => VehicleType.Bus;

    protected override string Details => $"Seats: {NumberOfSeats}";

    public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
        : base(registrationNumber, color, numberOfWheels)
    {
        // TODO: Phase 2 - Validate the number of seats with a range and a clear error message.
        NumberOfSeats = numberOfSeats;
    }
}
