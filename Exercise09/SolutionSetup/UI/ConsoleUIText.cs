namespace SolutionSetup.UI;

/// <summary>
/// Text shown by <see cref="ConsoleUI"/>. Shared by the UI code and its tests so the wording is defined in one place.
/// </summary>
public static class ConsoleUIText
{
    public const string MenuTitle = "Welcome to your Garage 2.0";
    public const string InvalidChoice = "Invalid choice. Please select a valid menu option.";
    public const string PressEnterToContinue = "Press Enter to continue...";
    public const string NoVehiclesParked = "No vehicles are parked in the garage.";
    public const string NoGarage = "No garage has been created yet.";
    public const string NoGaragePleaseCreate = NoGarage + " Please create a garage first.";
    public const string InputEndedBeforeGarageCreated = "Input ended before the garage was created.";
    public const string InputEndedBeforeVehicleParked = "Input ended before the vehicle was parked.";
}
