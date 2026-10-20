using SolutionSetup.Vehicles;
using System;
using System.Globalization;
using System.IO;

namespace SolutionSetup.UI
{
    /// <summary>
    /// Text-based user interface for the garage. Reads menu choices and vehicle details from the input
    /// and writes results to the output. Both streams are injectable so the UI can be tested.
    /// </summary>
    public partial class ConsoleUI
    {
        private readonly TextReader _input;
        private readonly TextWriter _output;
        // The garage is null until the user creates one (menu option 1).
        private Garage? _garage;

        /// <summary>
        /// Creates a console UI that reads from <see cref="Console.In"/> and writes to <see cref="Console.Out"/>.
        /// </summary>
        public ConsoleUI()
            : this(Console.In, Console.Out)
        {
        }

        /// <summary>
        /// Creates a console UI that uses the given input and output streams.
        /// </summary>
        /// <param name="input">Source of user input.</param>
        /// <param name="output">Destination for menu text and messages.</param>
        internal ConsoleUI(TextReader input, TextWriter output)
        {
            _input = input;
            _output = output;
        }

        /// <summary>
        /// Runs the main menu loop. Prints the menu, reads a choice, and dispatches it to the matching
        /// handler until the user selects 0. Unexpected exceptions are caught so the loop keeps running.
        /// </summary>
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                string choice = ReadLineOrEmpty();

                try
                {
                    // Each menu option maps to one handler method below.
                    switch (choice)
                    {
                        case "1":
                            CreateGarage();
                            break;
                        case "2":
                            PopulateGarage();
                            break;
                        case "3":
                            ParkVehicle();
                            break;
                        case "4":
                            RemoveVehicle();
                            break;
                        case "5":
                            ListVehicles();
                            break;
                        case "6":
                            ListVehicleCounts();
                            break;
                        case "7":
                            FindVehicle();
                            break;
                        case "8":
                            SearchVehicles();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            _output.WriteLine("Invalid choice. Please select a valid menu option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Prints the numbered main menu (options 1 to 8 and 0 for exit).
        /// </summary>
        private void PrintMenu()
        {
            _output.WriteLine("Garage 1.0");
            _output.WriteLine("1. Create garage");
            _output.WriteLine("2. Populate garage with sample vehicles");
            _output.WriteLine("3. Park vehicle");
            _output.WriteLine("4. Remove vehicle");
            _output.WriteLine("5. List all vehicles");
            _output.WriteLine("6. List vehicle count by type");
            _output.WriteLine("7. Find vehicle by registration number");
            _output.WriteLine("8. Search vehicles by properties");
            _output.WriteLine("0. Exit");
        }

        /// <summary>
        /// Reads one line of input. Returns an empty string at end of input so callers never receive null.
        /// </summary>
        private string ReadLineOrEmpty() => _input.ReadLine() ?? string.Empty;

        /// <summary>
        /// Returns true and the current garage if one exists. Otherwise prints a message and returns false.
        /// Menu handlers use this to stop early when no garage has been created.
        /// </summary>
        private bool TryGetGarage(out Garage garage)
        {
            if (_garage is null)
            {
                _output.WriteLine("No garage has been created yet. Please create a garage first.");
                garage = null!;
                return false;
            }

            garage = _garage;
            return true;
        }

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

            OperationResult result = garage.Populate(SampleData.CreateSampleVehicles());
            _output.WriteLine(result.Message);
        }

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
        /// Prints a prompt and reads a whole number. Throws <see cref="FormatException"/> if the input is not a number.
        /// </summary>
        private int ReadIntPrompt(string prompt)
        {
            _output.WriteLine(prompt);
            string line = ReadLineOrEmpty();
            if (!int.TryParse(line, out int value))
            {
                throw new FormatException($"'{line}' is not a valid whole number.");
            }

            return value;
        }

        /// <summary>
        /// Prints a prompt and reads a decimal number. Throws <see cref="FormatException"/> if the input is not a number.
        /// </summary>
        private double ReadDoublePrompt(string prompt)
        {
            _output.WriteLine(prompt);
            string line = ReadLineOrEmpty();
            if (!double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                throw new FormatException($"'{line}' is not a valid number.");
            }

            return value;
        }

        /// <summary>
        /// Prompts for a fuel type (Gasoline or Diesel). Throws <see cref="FormatException"/> if the input is not valid.
        /// </summary>
        private FuelType ReadFuelTypePrompt()
        {
            _output.WriteLine("Enter fuel type (Gasoline/Diesel):");
            string line = ReadLineOrEmpty();
            if (!Enum.TryParse(line, ignoreCase: true, out FuelType fuelType))
            {
                throw new FormatException($"'{line}' is not a valid fuel type.");
            }

            return fuelType;
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

            Vehicle[] vehicles = garage.ListAllVehicles();
            if (vehicles.Length == 0)
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
        /// Menu option 6. Prints the number of parked vehicles for each vehicle type.
        /// </summary>
        internal void ListVehicleCounts()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            int[] counts = garage.ListVehicleCountByType();
            int total = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                total += counts[i];
            }

            if (total == 0)
            {
                _output.WriteLine("No vehicles are parked in the garage.");
                return;
            }

            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > 0)
                {
                    _output.WriteLine($"{(VehicleType)i}: {counts[i]}");
                }
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
