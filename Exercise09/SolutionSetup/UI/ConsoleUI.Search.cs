using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup.UI;

public partial class ConsoleUI
{
    /// <summary>
    /// Menu option 8. Reads optional search filters and prints the vehicles that match all of them.
    /// </summary>
    internal void SearchVehicles()
    {
        if (!EnsureGarage())
        {
            return;
        }

        _output.WriteLine("Enter criteria as property=value pairs separated by spaces, e.g. color=Black type=Car (leave blank for all vehicles).");
        _output.WriteLine($"Properties: registration, color, wheels, type ({string.Join(", ", Enum.GetValues<VehicleType>())})");
        string line = ReadLineOrEmpty();

        if (!TryParseSearchCriteria(line, out SearchCriteria criteria))
        {
            return;
        }

        PrintSearchResults(_handler.SearchVehicles(criteria));
    }

    // Parses one line of property=value pairs; returns false (after printing an error) if any pair is invalid.
    private bool TryParseSearchCriteria(string line, out SearchCriteria criteria)
    {
        criteria = new SearchCriteria();

        foreach (string pair in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            int separatorIndex = pair.IndexOf('=');
            if (separatorIndex <= 0 || separatorIndex == pair.Length - 1)
            {
                _output.WriteLine($"Invalid search criterion '{pair}'. Use property=value, e.g. color=Black.");
                return false;
            }

            if (!TryApplySearchCriterion(criteria, pair[..separatorIndex], pair[(separatorIndex + 1)..]))
            {
                return false;
            }
        }

        return true;
    }

    // Applies a single property=value pair to the criteria; returns false (after printing an error) if it is invalid.
    private bool TryApplySearchCriterion(SearchCriteria criteria, string property, string value)
    {
        switch (property.ToLowerInvariant())
        {
            case "registration":
                criteria.RegistrationNumber = value.Trim();
                return true;
            case "color":
                criteria.Color = value;
                return true;
            case "wheels":
                if (!int.TryParse(value, out int numberOfWheels))
                {
                    _output.WriteLine($"Invalid number of wheels '{value}'. Please enter a whole number.");
                    return false;
                }

                criteria.NumberOfWheels = numberOfWheels;
                return true;
            case "type":
                if (!Enum.TryParse(value, ignoreCase: true, out VehicleType type) || !Enum.IsDefined(type))
                {
                    _output.WriteLine($"Invalid vehicle type '{value}'.");
                    return false;
                }

                criteria.Type = type;
                return true;
            default:
                _output.WriteLine($"Unknown search property '{property}'. Valid properties: registration, color, wheels, type.");
                return false;
        }
    }

    // Prints each matching vehicle, or a message when nothing matched.
    private void PrintSearchResults(IReadOnlyList<IVehicle> results)
    {
        if (results.Count == 0)
        {
            _output.WriteLine("No vehicles matched the given criteria.");
            return;
        }

        foreach (IVehicle vehicle in results)
        {
            _output.WriteLine(vehicle.ToString());
        }
    }
}
