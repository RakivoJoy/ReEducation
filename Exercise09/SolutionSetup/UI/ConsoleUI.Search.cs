using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup.UI;

public partial class ConsoleUI
{
    /// <summary>
    /// Menu option 8. Reads optional search filters and prints the vehicles that match all of them.
    /// </summary>
    internal void SearchVehicles()
    {

        _output.WriteLine(ConsoleUIText.SearchCriteriaPrompt);
        _output.WriteLine($"Properties: {string.Join(", ", SearchCriteria.PropertyNames)} ({string.Join(", ", VehicleTypes.Select(info => info.Type))})");
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

            if (!criteria.TrySetFilter(pair[..separatorIndex], pair[(separatorIndex + 1)..], out string error))
            {
                _output.WriteLine(error);
                return false;
            }
        }

        return true;
    }

    // Prints each matching vehicle, or a message when nothing matched.
    private void PrintSearchResults(IReadOnlyList<IVehicle> results)
    {
        if (results.Count == 0)
        {
            _output.WriteLine(ConsoleUIText.NoVehiclesMatched);
            return;
        }

        foreach (IVehicle vehicle in results)
        {
            _output.WriteLine(vehicle.ToString());
        }
    }
}
