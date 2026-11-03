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
    public const string SamplesAlreadyAdded = "Sample vehicles are already added to this garage.";
    public const string NoVehiclesMatched = "No vehicles matched the given criteria.";
    public const string SearchCriteriaPrompt = "Enter criteria as property=value pairs separated by spaces, e.g. color=Black type=Car (leave blank for all vehicles).";
    public const string SelectVehicleType = "Select vehicle type:";
    public const string EnterRegistrationNumber = "Enter registration number:";
    public const string EnterRegistrationNumberToRemove = "Enter registration number to remove:";
    public const string EnterRegistrationNumberToFind = "Enter registration number to find:";
    public const string EnterColor = "Enter color:";
    public const string EnterNumberOfWheels = "Enter number of wheels:";
    public const string InvalidNumberOfWheels = "Invalid number of wheels. Please enter a whole number.";
    public const string InvalidWholeNumber = "Invalid input. Please enter a whole number.";
    public const string ConfirmLoadOverwrite = "The current garage has parked vehicles. Loading will replace them. Continue? (y/n):";
    public const string LoadCancelled = "Load cancelled. The current garage was kept.";
}
