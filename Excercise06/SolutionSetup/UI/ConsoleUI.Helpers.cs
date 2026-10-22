using SolutionSetup.Vehicles;
using System;
using System.Globalization;

namespace SolutionSetup.UI;

/// <summary>
/// Shared input helpers: reading lines, checking that a garage exists, and parsing typed prompts.
/// </summary>
public partial class ConsoleUI
{
    /// <summary>
    /// Reads one line of input. Returns an empty string at end of input so callers never receive null.
    /// </summary>
    private string ReadLineOrEmpty() => _input.ReadLine() ?? string.Empty;

    /// <summary>
    /// Returns true and the current garage if one exists. Otherwise prints a message and returns false.
    /// Menu handlers use this to stop early when no garage has been created.
    /// </summary>
    private bool TryGetGarage(out Garage garage)
    {
        if (_garage is null)
        {
            _output.WriteLine("No garage has been created yet. Please create a garage first.");
            garage = null!;
            return false;
        }

        garage = _garage;
        return true;
    }

    /// <summary>
    /// Prints a prompt and reads a whole number. Throws <see cref="FormatException"/> if the input is not a number.
    /// </summary>
    private int ReadIntPrompt(string prompt)
    {
        _output.WriteLine(prompt);
        string line = ReadLineOrEmpty();
        if (!int.TryParse(line, out int value))
        {
            throw new FormatException($"'{line}' is not a valid whole number.");
        }

        return value;
    }

    /// <summary>
    /// Prints a prompt and reads a decimal number. Throws <see cref="FormatException"/> if the input is not a number.
    /// </summary>
    private double ReadDoublePrompt(string prompt)
    {
        _output.WriteLine(prompt);
        string line = ReadLineOrEmpty();
        if (!double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
        {
            throw new FormatException($"'{line}' is not a valid number.");
        }

        return value;
    }

    /// <summary>
    /// Prompts for a fuel type (Gasoline or Diesel). Throws <see cref="FormatException"/> if the input is not valid.
    /// </summary>
    private FuelType ReadFuelTypePrompt()
    {
        _output.WriteLine("Enter fuel type (Gasoline/Diesel):");
        string line = ReadLineOrEmpty();
        if (!Enum.TryParse(line, ignoreCase: true, out FuelType fuelType) ||
            !Enum.IsDefined(typeof(FuelType), fuelType))
        {
            throw new FormatException($"'{line}' is not a valid fuel type.");
        }

        return fuelType;
    }
}
