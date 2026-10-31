using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup.UI;

/// <summary>
/// Menu handlers that create, populate and report on the garage as a whole.
/// </summary>
public partial class ConsoleUI
{
    /// <summary>
    /// Menu option 1. Asks for a capacity and creates a new garage. Replaces any existing garage.
    /// </summary>
    internal void CreateGarage()
    {
        // TODO: Phase 4 - Show the valid capacity range (e.g. 1-1000), handle numbers too big for an int, and re-prompt on invalid input.
        _output.WriteLine("Enter garage capacity (positive whole number):");
        string line = ReadLineOrEmpty();

        if (!int.TryParse(line, out int capacity))
        {
            _output.WriteLine($"Invalid capacity '{line}'. Please enter a positive whole number.");
            return;
        }

        OperationResult result = _handler.CreateGarage(capacity);
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 2. Adds the sample vehicles to the garage and prints the result.
    /// </summary>
    internal void PopulateGarage()
    {
        if (!EnsureGarage())
        {
            return;
        }

        // TODO: Phase 4 - Show "Sample vehicles are already added" when populate is used a second time.
        OperationResult result = _handler.PopulateGarage();
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 6. Prints the number of parked vehicles for each vehicle type.
    /// </summary>
    internal void ListVehicleCounts()
    {
        if (!EnsureGarage())
        {
            return;
        }

        Dictionary<VehicleType, int> counts = _handler.ListVehicleCounts();

        if (counts.Values.Sum() == 0)
        {
            _output.WriteLine("No vehicles are parked in the garage.");
            return;
        }

        foreach (var entry in counts.OrderBy(e => e.Key))
        {
            _output.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }
}
