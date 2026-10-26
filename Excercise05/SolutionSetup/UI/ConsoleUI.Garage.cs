using SolutionSetup.Vehicles;
using System;

namespace SolutionSetup.UI
{
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

            // Reject input that is not a whole number.
            if (!int.TryParse(line, out int capacity))
            {
                _output.WriteLine($"Invalid capacity '{line}'. Please enter a positive whole number.");
                return;
            }

            // The Garage constructor rejects non-positive capacities.
            try
            {
                _garage = new Garage(capacity);
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
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            // Adds the built-in sample vehicles; the result reports how many were parked.
            OperationResult result = garage.Populate(SampleData.CreateSampleVehicles());
            _output.WriteLine(result.Message);
        }

        /// <summary>
        /// Menu option 6. Prints the number of parked vehicles for each vehicle type.
        /// </summary>
        internal void ListVehicleCounts()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            // Count per vehicle type, indexed by VehicleType.
            int[] counts = garage.ListVehicleCountByType();
            int total = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                total += counts[i];
            }

            // Nothing to list when the garage is empty.
            if (total == 0)
            {
                _output.WriteLine("No vehicles are parked in the garage.");
                return;
            }

            // Print only the types that have at least one parked vehicle.
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > 0)
                {
                    _output.WriteLine($"{(VehicleType)i}: {counts[i]}");
                }
            }
        }
    }
}
