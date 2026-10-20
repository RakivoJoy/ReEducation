using SolutionSetup.Vehicles;
using System;

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

        public OperationResult Populate(Vehicle[] vehicles)
        {
            ArgumentNullException.ThrowIfNull(vehicles);

            int parked = 0;
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (AddVehicle(vehicles[i]).Success)
                {
                    parked++;
                }
            }

            string message = $"Parked {parked} of {vehicles.Length} vehicles.";
            return parked == vehicles.Length ? OperationResult.Ok(message) : OperationResult.Fail(message);
        }

        public Vehicle[] ListAllVehicles()
        {
            Vehicle[] result = new Vehicle[CountParked()];
            int next = 0;
            for (int i = 0; i < _vehicles.Length; i++)
            {
                if (_vehicles[i] is { } vehicle)
                {
                    result[next++] = vehicle;
                }
            }

            return result;
        }

        // The returned array is indexed by (int)VehicleType.
        public int[] ListVehicleCountByType()
        {
            int[] counts = new int[Enum.GetNames(typeof(VehicleType)).Length];
            for (int i = 0; i < _vehicles.Length; i++)
            {
                if (_vehicles[i] is { } vehicle)
                {
                    counts[(int)vehicle.Type]++;
                }
            }

            return counts;
        }

        public Vehicle? FindByRegistration(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            return index < 0 ? null : _vehicles[index];
        }

        public Vehicle[] SearchByProperties(SearchCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            Vehicle[] all = ListAllVehicles();
            Vehicle[] matches = new Vehicle[all.Length];
            int count = 0;
            for (int i = 0; i < all.Length; i++)
            {
                Vehicle vehicle = all[i];
                if ((criteria.RegistrationNumber is null || string.Equals(vehicle.RegistrationNumber, criteria.RegistrationNumber, StringComparison.OrdinalIgnoreCase)) &&
                    (criteria.Color is null || string.Equals(vehicle.Color, criteria.Color, StringComparison.OrdinalIgnoreCase)) &&
                    (criteria.NumberOfWheels is null || vehicle.NumberOfWheels == criteria.NumberOfWheels) &&
                    (criteria.Type is null || vehicle.Type == criteria.Type))
                {
                    matches[count++] = vehicle;
                }
            }

            Vehicle[] result = new Vehicle[count];
            Array.Copy(matches, result, count);
            return result;
        }

        private int CountParked()
        {
            int count = 0;
            for (int i = 0; i < _vehicles.Length; i++)
            {
                if (_vehicles[i] is not null)
                {
                    count++;
                }
            }

            return count;
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
