using SolutionSetup.Vehicles;

namespace SolutionSetup.UI
{
    public partial class ConsoleUI
    {
        private readonly TextReader _input;
        private readonly TextWriter _output;
        private Garage? _garage;

        public ConsoleUI()
            : this(Console.In, Console.Out)
        {
        }

        internal ConsoleUI(TextReader input, TextWriter output)
        {
            _input = input;
            _output = output;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                string choice = ReadLineOrEmpty();

                try
                {
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

        private string ReadLineOrEmpty() => _input.ReadLine() ?? string.Empty;

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

        internal void PopulateGarage()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            OperationResult result = garage.Populate(SampleData.CreateSampleVehicles());
            _output.WriteLine(result.Message);
        }

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

        private double ReadDoublePrompt(string prompt)
        {
            _output.WriteLine(prompt);
            string line = ReadLineOrEmpty();
            if (!double.TryParse(line, out double value))
            {
                throw new FormatException($"'{line}' is not a valid number.");
            }

            return value;
        }

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

        internal void ListVehicleCounts()
        {
            if (!TryGetGarage(out Garage garage))
            {
                return;
            }

            Dictionary<VehicleType, int> counts = garage.ListVehicleCountByType();
            if (counts.Count == 0)
            {
                _output.WriteLine("No vehicles are parked in the garage.");
                return;
            }

            foreach (KeyValuePair<VehicleType, int> count in counts)
            {
                _output.WriteLine($"{count.Key}: {count.Value}");
            }
        }

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
