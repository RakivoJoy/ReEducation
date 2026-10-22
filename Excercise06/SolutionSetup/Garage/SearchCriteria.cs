using SolutionSetup.Vehicles;

namespace SolutionSetup;

/// <summary>
/// Optional filters for <see cref="Garage.SearchByProperties"/>. A null property means "do not filter on this".
/// </summary>
public class SearchCriteria
{
    /// <summary>Registration number to match, ignoring case.</summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>Color to match, ignoring case.</summary>
    public string? Color { get; set; }

    /// <summary>Exact number of wheels to match.</summary>
    public int? NumberOfWheels { get; set; }

    /// <summary>Vehicle type to match.</summary>
    public VehicleType? Type { get; set; }
}
