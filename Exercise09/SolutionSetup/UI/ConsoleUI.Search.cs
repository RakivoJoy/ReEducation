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
        if (!EnsureGarage())
        {
            return;
        }

        _output.WriteLine("Enter criteria as property=value pairs separated by spaces, e.g. color=Black type=Car (leave blank for all vehicles).");
        _output.WriteLine($"Properties: {string.Join(", ", SearchProperties.All.Select(property => property.Name))} ({string.Join(", ", VehicleTypes.Select(info => info.Type))})");
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

    // Applies a single property=value pair to the criteria using the shared registry; returns false (after printing an error) if it is invalid.
    private bool TryApplySearchCriterion(SearchCriteria criteria, string property, string value)
    {
        SearchProperty? searchProperty = SearchProperties.Find(property);
        if (searchProperty is null)
        {
            _output.WriteLine($"Unknown search property '{property}'. Valid properties: {string.Join(", ", SearchProperties.All.Select(p => p.Name))}.");
            return false;
        }

        object? parsed = searchProperty.Parse.Invoke(value);
        if (parsed is null)
        {
            _output.WriteLine(searchProperty.InvalidValueMessage.Invoke(value));
            return false;
        }

        criteria.SetFilter(searchProperty.Name, parsed);
        return true;
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
