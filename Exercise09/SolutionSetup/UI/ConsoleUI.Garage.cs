using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolutionSetup.UI;

/// <summary>
/// Menu handlers that create, populate and report on the garage as a whole.
/// </summary>
public partial class ConsoleUI
{
    private bool _samplesAdded;

    /// <summary>
    /// Menu option 1. Asks for a capacity and creates a new garage. Replaces any existing garage.
    /// </summary>
    internal void CreateGarage()
    {
        try
        {
            int capacity = ReadValidated<int>(
                $"Enter garage capacity (1-{Garage<IVehicle>.MaxCapacity}):",
                line => $"Invalid capacity '{line}'. Please enter a whole number (1-{Garage<IVehicle>.MaxCapacity}).",
                (string text, out int value) => int.TryParse(text, out value) && value <= Garage<IVehicle>.MaxCapacity);

            OperationResult result = _handler.CreateGarage(capacity);
            if (result.Success)
            {
                _samplesAdded = false;
            }

            _output.WriteLine(result.Message);
        }
        catch (EndOfStreamException)
        {
            _output.WriteLine(ConsoleUIText.InputEndedBeforeGarageCreated);
        }
    }

    /// <summary>
    /// Menu option 2. Adds the sample vehicles to the garage and prints the result.
    /// </summary>
    internal void PopulateGarage()
    {
        if (_samplesAdded)
        {
            _output.WriteLine(ConsoleUIText.SamplesAlreadyAdded);
            return;
        }

        OperationResult result = _handler.PopulateGarage();
        _samplesAdded = true;
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 6. Prints the number of parked vehicles for each vehicle type.
    /// </summary>
    internal void ListVehicleCounts()
    {
        Dictionary<VehicleType, int> counts = _handler.ListVehicleCounts();

        if (counts.Values.Sum() == 0)
        {
            _output.WriteLine(ConsoleUIText.NoVehiclesParked);
            return;
        }

        foreach (var entry in counts.OrderBy(e => e.Key))
        {
            _output.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }
}
