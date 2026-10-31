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
        _output.WriteLine("Enter garage capacity (positive whole number):");
        string line = ReadLineOrEmpty();

        if (!int.TryParse(line, out int capacity))
        {
            _output.WriteLine($"Invalid capacity '{line}'. Please enter a positive whole number.");
            return;
        }

        try
        {
            _garage = new Garage<IVehicle>(capacity);
            _output.WriteLine($"Garage created with capacity {capacity}.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _output.WriteLine($"Could not create garage: {ex.Message}");
        }
    }

    /// <summary>
    /// Menu option 2. Adds the sample vehicles to the garage and prints the result.
    /// </summary>
    internal void PopulateGarage()
    {
        if (!TryGetGarage(out Garage<IVehicle> garage))
        {
            return;
        }

        OperationResult result = garage.Populate(SampleData.CreateSampleVehicles());
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 6. Prints the number of parked vehicles for each vehicle type.
    /// </summary>
    internal void ListVehicleCounts()
    {
        if (!TryGetGarage(out Garage<IVehicle> garage))
        {
            return;
        }

        Dictionary<VehicleType, int> counts = garage.ListVehicleCountByType();

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
