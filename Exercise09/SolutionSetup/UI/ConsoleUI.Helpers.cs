using SolutionSetup.Vehicles;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

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
    /// Returns true if a garage exists. Otherwise prints a message and returns false.
    /// Menu handlers use this to stop early when no garage has been created.
    /// </summary>
    private bool EnsureGarage()
    {
        if (!_handler.HasGarage)
        {
            _output.WriteLine(ConsoleUIText.NoGaragePleaseCreate);
            return false;
        }

        return true;
    }

    //TODO: Verify this is used in the UI and if not, remove it. It is used in the tests, but not in the UI.
    private delegate bool TryParser<T>(string text, out T value);

    /// <summary>
    /// Prints a prompt and keeps asking until the input can be parsed. Throws <see cref="EndOfStreamException"/> if the input ends first.
    /// </summary>
    private T ReadValidated<T>(string prompt, Func<string, string> invalidMessage, TryParser<T> tryParse)
    {
        _output.WriteLine(prompt);
        while (true)
        {
            string line = _input.ReadLine() ?? throw new EndOfStreamException();
            if (tryParse(line, out T value))
            {
                return value;
            }

            _output.WriteLine(invalidMessage(line));
            _output.WriteLine(prompt);
        }
    }

    /// <summary>
    /// Prints a prompt and reads a whole number, asking again until the input is valid.
    /// </summary>
    private int ReadIntPrompt(string prompt, string invalidMessage = ConsoleUIText.InvalidWholeNumber)
    {
        return ReadValidated<int>(prompt, _ => invalidMessage, (string text, out int value) => int.TryParse(text, out value));
    }

    /// <summary>
    /// Prints a prompt and reads a decimal number, asking again until the input is valid.
    /// </summary>
    private double ReadDoublePrompt(string prompt)
    {
        return ReadValidated<double>(prompt, line => $"Invalid input: '{line}' is not a valid number.", (string text, out double value) => double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value));
    }

    /// <summary>
    /// Prompts for a fuel type (Gasoline or Diesel) and asks again until the input is valid.
    /// </summary>
    private FuelType ReadFuelTypePrompt()
    {
        return ReadValidated<FuelType>(
            "Enter fuel type (Gasoline/Diesel):",
            line => $"Invalid input: '{line}' is not a valid fuel type.",
            (string text, out FuelType value) =>
            {
                string line = text.Trim();
                string candidate = line.Length == 1
                    ? Enum.GetNames<FuelType>().FirstOrDefault(name => name.StartsWith(line, StringComparison.OrdinalIgnoreCase)) ?? line
                    : line;
                return Enum.TryParse(candidate, ignoreCase: true, out value) && Enum.IsDefined(value);
            });
    }
}
