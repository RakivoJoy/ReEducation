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

        public bool AddVehicle(Vehicle vehicle) => throw new NotImplementedException();

        public bool RemoveVehicleByRegistration(string registrationNumber) => throw new NotImplementedException();

        public IEnumerable<Vehicle> ListAllVehicles() => throw new NotImplementedException();

        public Dictionary<VehicleType, int> ListVehicleCountByType() => throw new NotImplementedException();

        public Vehicle? FindByRegistration(string registrationNumber) => throw new NotImplementedException();

        public IEnumerable<Vehicle> SearchByProperties(SearchCriteria criteria) => throw new NotImplementedException();
    }
}
