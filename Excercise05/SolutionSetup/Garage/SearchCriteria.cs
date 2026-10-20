using SolutionSetup.Vehicles;

namespace SolutionSetup
{
    public class SearchCriteria
    {
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
        public int? NumberOfWheels { get; set; }
        public VehicleType? Type { get; set; }
    }
}
