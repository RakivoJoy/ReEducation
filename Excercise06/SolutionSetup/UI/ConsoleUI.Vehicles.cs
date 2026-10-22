using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup.UI
{
    /// <summary>
    /// Menu handlers that park, remove, list and find individual vehicles.
    /// </summary>
    public partial class ConsoleUI
    {
        /// <summary>
        /// Menu option 3. Asks for a vehicle type and its details, then parks the vehicle.
        /// Invalid input is reported to the user and nothing is parked.
        /// </summary>
        internal void ParkVehicle()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            _output.WriteLine("Select vehicle type:");
            _output.WriteLine("1. Motorcycle");
            _output.WriteLine("2. Airplane");
            _output.WriteLine("3. Car");
            _output.WriteLine("4. Bus");
            _output.WriteLine("5. Boat");
            _output.WriteLine("6. Truck");
            string typeChoice = ReadLineOrEmpty();

            _output.WriteLine("Enter registration number:");
            string registrationNumber = ReadLineOrEmpty();

            _output.WriteLine("Enter color:");
            string color = ReadLineOrEmpty();

            _output.WriteLine("Enter number of wheels:");
            if (!int.TryParse(ReadLineOrEmpty(), out int numberOfWheels))
            {
                _output.WriteLine("Invalid number of wheels. Please enter a whole number.");
                return;
            }

            try
            {
                Vehicle vehicle = typeChoice switch
                {
                    "1" => new Motorcycle(registrationNumber, color, numberOfWheels, ReadIntPrompt("Enter cylinder volume (cc):")),
                    "2" => new Airplane(registrationNumber, color, numberOfWheels, ReadIntPrompt("Enter number of engines:")),
                    "3" => new Car(registrationNumber, color, numberOfWheels, ReadFuelTypePrompt()),
                    "4" => new Bus(registrationNumber, color, numberOfWheels, ReadIntPrompt("Enter number of seats:")),
                    "5" => new Boat(registrationNumber, color, numberOfWheels, ReadDoublePrompt("Enter length (m):")),
                    "6" => new Truck(registrationNumber, color, numberOfWheels, ReadIntPrompt("Enter load capacity (kg):")),
                    _ => throw new FormatException($"'{typeChoice}' is not a valid vehicle type option.")
                };

                OperationResult result = garage.AddVehicle(vehicle);
                _output.WriteLine(result.Message);
            }
            catch (FormatException ex)
            {
                _output.WriteLine($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                _output.WriteLine($"Could not park vehicle: {ex.Message}");
            }
        }

        /// <summary>
        /// Menu option 4. Asks for a registration number and removes the matching vehicle.
        /// </summary>
        internal void RemoveVehicle()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            _output.WriteLine("Enter registration number to remove:");
            string registrationNumber = ReadLineOrEmpty();

            OperationResult result = garage.RemoveVehicleByRegistration(registrationNumber);
            _output.WriteLine(result.Message);
        }

        /// <summary>
        /// Menu option 5. Prints every parked vehicle, or a message if the garage is empty.
        /// </summary>
        internal void ListVehicles()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            List<Vehicle> vehicles = garage.ListAllVehicles();
            if (vehicles.Count == 0)
            {
                _output.WriteLine("No vehicles are parked in the garage.");
                return;
            }

            foreach (Vehicle vehicle in vehicles)
            {
                _output.WriteLine(vehicle.ToString());
            }
        }

        /// <summary>
        /// Menu option 7. Asks for a registration number and prints the matching vehicle, if any.
        /// </summary>
        internal void FindVehicle()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            _output.WriteLine("Enter registration number to find:");
            string registrationNumber = ReadLineOrEmpty();

            Vehicle? vehicle = garage.FindByRegistration(registrationNumber);
            if (vehicle is null)
            {
                _output.WriteLine($"No vehicle with registration number {registrationNumber} was found.");
                return;
            }

            _output.WriteLine(vehicle.ToString());
        }
    }
}
