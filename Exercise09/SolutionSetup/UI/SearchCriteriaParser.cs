using SolutionSetup.Vehicles;
using System;
using System.Linq;

namespace SolutionSetup.UI;

/// <summary>
/// Turns console text such as <c>color=Black</c> into typed values on a <see cref="SearchCriteria"/>,
/// and owns the messages shown for unknown properties or invalid values.
/// </summary>
public static class SearchCriteriaParser
{
    /// <summary>Sets the named property on the criteria from user text. Returns false with an error message when invalid.</summary>
    public static bool TrySetFilter(SearchCriteria criteria, string property, string text, out string error)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        switch (property.ToLowerInvariant())
        {
            case "registration":
                criteria.RegistrationNumber = text.Trim();
                break;
            case "color":
                criteria.Color = text;
                break;
            case "wheels" when int.TryParse(text, out int wheels):
                criteria.NumberOfWheels = wheels;
                break;
            case "type" when Enum.TryParse(text, true, out VehicleType type) && Enum.IsDefined(type):
                criteria.Type = type;
                break;
            case "fuel" when Enum.TryParse(text, true, out FuelType fuel) && Enum.IsDefined(fuel):
                criteria.FuelType = fuel;
                break;
            case "size" when IntRange.TryParse(text, out IntRange? range):
                criteria.ParkingSize = range;
                break;
            case "wheels" or "type" or "fuel" or "size":
                error = InvalidMessage(property, text);
                return false;
            default:
                error = $"Unknown search property '{property}'. Valid properties: {string.Join(", ", SearchCriteria.PropertyNames)}.";
                return false;
        }

        error = string.Empty;
        return true;
    }

    private static string InvalidMessage(string property, string text) => property.ToLowerInvariant() switch
    {
        "wheels" => $"Invalid number of wheels '{text}'. Please enter a whole number.",
        "type" => $"Invalid vehicle type '{text}'.",
        "fuel" => $"Invalid fuel type '{text}'. Valid fuel types: {string.Join(", ", Enum.GetNames<FuelType>())}.",
        _ => $"Invalid parking size '{text}'. Use a whole number or a range such as 10-30.",
    };
}
