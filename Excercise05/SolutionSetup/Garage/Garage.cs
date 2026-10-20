using SolutionSetup.Vehicles;

namespace SolutionSetup
{
    public class Garage
    {
        private readonly Vehicle?[] _vehicles;

        public int Capacity => _vehicles.Length;

        public Garage(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
            }

            _vehicles = new Vehicle?[capacity];
        }

        public OperationResult AddVehicle(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            if (FindByRegistration(vehicle.RegistrationNumber) is not null)
            {
                return OperationResult.Fail($"A vehicle with registration number {vehicle.RegistrationNumber} is already parked.");
            }

            int index = Array.IndexOf(_vehicles, null);
            if (index < 0)
            {
                return OperationResult.Fail("The garage is full.");
            }

            _vehicles[index] = vehicle;
            return OperationResult.Ok($"Vehicle {vehicle.RegistrationNumber} parked.");
        }

        public OperationResult RemoveVehicleByRegistration(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            if (index < 0)
            {
                return OperationResult.Fail($"No vehicle with registration number {registrationNumber} was found.");
            }

            _vehicles[index] = null;
            return OperationResult.Ok($"Vehicle {registrationNumber} removed.");
        }

        public OperationResult Populate(IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(vehicles);

            var vehicleList = vehicles.ToList();
            int parked = 0;
            foreach (Vehicle vehicle in vehicleList)
            {
                if (AddVehicle(vehicle).Success)
                {
                    parked++;
                }
            }

            string message = $"Parked {parked} of {vehicleList.Count} vehicles.";
            return parked == vehicleList.Count ? OperationResult.Ok(message) : OperationResult.Fail(message);
        }

        public List<Vehicle> ListAllVehicles() => _vehicles.OfType<Vehicle>().ToList();

        public Dictionary<VehicleType, int> ListVehicleCountByType() =>
            ListAllVehicles()
                .GroupBy(vehicle => vehicle.Type)
                .ToDictionary(group => group.Key, group => group.Count());

        public Vehicle? FindByRegistration(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            return index < 0 ? null : _vehicles[index];
        }

        public List<Vehicle> SearchByProperties(SearchCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return ListAllVehicles()
                .Where(vehicle =>
                    (criteria.RegistrationNumber is null || string.Equals(vehicle.RegistrationNumber, criteria.RegistrationNumber, StringComparison.OrdinalIgnoreCase)) &&
                    (criteria.Color is null || string.Equals(vehicle.Color, criteria.Color, StringComparison.OrdinalIgnoreCase)) &&
                    (criteria.NumberOfWheels is null || vehicle.NumberOfWheels == criteria.NumberOfWheels) &&
                    (criteria.Type is null || vehicle.Type == criteria.Type))
                .ToList();
        }

        private int FindIndex(string registrationNumber)
        {
            for (int i = 0; i < _vehicles.Length; i++)
            {
                if (_vehicles[i] is { } vehicle &&
                    string.Equals(vehicle.RegistrationNumber, registrationNumber, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
