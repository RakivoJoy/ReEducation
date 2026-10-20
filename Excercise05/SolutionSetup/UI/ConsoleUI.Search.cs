using SolutionSetup.Vehicles;
using System;

namespace SolutionSetup.UI
{
    public partial class ConsoleUI
    {
        /// <summary>
        /// Menu option 8. Reads optional search filters and prints the vehicles that match all of them.
        /// </summary>
        internal void SearchVehicles()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            if (!TryReadSearchCriteria(out SearchCriteria criteria))
            {
                return;
            }

            PrintSearchResults(garage.SearchByProperties(criteria));
        }

        // Reads all search inputs; returns false (after printing an error) if any input is invalid.
        private bool TryReadSearchCriteria(out SearchCriteria criteria)
        {
            criteria = null!;

            _output.WriteLine("Enter registration number (leave blank to skip):");
            string registrationNumber = ReadLineOrEmpty();

            _output.WriteLine("Enter color (leave blank to skip):");
            string color = ReadLineOrEmpty();

            if (!TryReadOptionalWheels(out int? numberOfWheels))
            {
                return false;
            }

            if (!TryReadOptionalType(out VehicleType? type))
            {
                return false;
            }

            criteria = new SearchCriteria
            {
                RegistrationNumber = string.IsNullOrWhiteSpace(registrationNumber) ? null : registrationNumber.Trim(),
                Color = string.IsNullOrWhiteSpace(color) ? null : color,
                NumberOfWheels = numberOfWheels,
                Type = type
            };
            return true;
        }

        // Reads an optional wheel count; blank input means "no filter".
        private bool TryReadOptionalWheels(out int? numberOfWheels)
        {
            numberOfWheels = null;

            _output.WriteLine("Enter number of wheels (leave blank to skip):");
            string wheelsLine = ReadLineOrEmpty();
            if (string.IsNullOrWhiteSpace(wheelsLine))
            {
                return true;
            }

            if (!int.TryParse(wheelsLine, out int parsedWheels))
            {
                _output.WriteLine($"Invalid number of wheels '{wheelsLine}'. Please enter a whole number.");
                return false;
            }

            numberOfWheels = parsedWheels;
            return true;
        }

        // Reads an optional vehicle type; blank input means "no filter".
        private bool TryReadOptionalType(out VehicleType? type)
        {
            type = null;

            _output.WriteLine("Enter vehicle type (leave blank to skip): Motorcycle, Airplane, Car, Bus, Boat, Truck");
            string typeLine = ReadLineOrEmpty();
            if (string.IsNullOrWhiteSpace(typeLine))
            {
                return true;
            }

            if (!Enum.TryParse(typeLine, ignoreCase: true, out VehicleType parsedType) ||
                !Enum.IsDefined(typeof(VehicleType), parsedType))
            {
                _output.WriteLine($"Invalid vehicle type '{typeLine}'.");
                return false;
            }

            type = parsedType;
            return true;
        }

        // Prints each matching vehicle, or a message when nothing matched.
        private void PrintSearchResults(Vehicle[] results)
        {
            if (results.Length == 0)
            {
                _output.WriteLine("No vehicles matched the given criteria.");
                return;
            }

            foreach (Vehicle vehicle in results)
            {
                _output.WriteLine(vehicle.ToString());
            }
        }
    }
}
