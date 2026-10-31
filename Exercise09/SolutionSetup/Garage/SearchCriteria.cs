using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Optional filters for <see cref="Garage{T}.SearchByProperties"/>. Filters are stored by the canonical name of a
/// <see cref="SearchProperty"/>; the typed properties are convenience accessors over the same filters.
/// A null or unset property means "do not filter on this".
/// </summary>
public class SearchCriteria
{
    private readonly Dictionary<string, object> _filters = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>The active filters, keyed by <see cref="SearchProperty.Name"/> and holding the typed filter values.</summary>
    public IReadOnlyDictionary<string, object> Filters => _filters;

    /// <summary>Sets the filter for a searchable property; passing null removes the filter.</summary>
    public void SetFilter(string propertyName, object? value)
    {
        if (value is null)
        {
            _filters.Remove(propertyName);
        }
        else
        {
            _filters[propertyName] = value;
        }
    }

    /// <summary>Registration number to match, ignoring case.</summary>
    public string? RegistrationNumber
    {
        get => _filters.TryGetValue("registration", out object? value) ? (string)value : null;
        set => SetFilter("registration", value);
    }

    /// <summary>Color to match, ignoring case.</summary>
    public string? Color
    {
        get => _filters.TryGetValue("color", out object? value) ? (string)value : null;
        set => SetFilter("color", value);
    }

    /// <summary>Exact number of wheels to match.</summary>
    public int? NumberOfWheels
    {
        get => _filters.TryGetValue("wheels", out object? value) ? (int)value : null;
        set => SetFilter("wheels", value);
    }

    /// <summary>Vehicle type to match.</summary>
    public VehicleType? Type
    {
        get => _filters.TryGetValue("type", out object? value) ? (VehicleType)value : null;
        set => SetFilter("type", value);
    }
}
