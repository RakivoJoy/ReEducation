using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup;

/// <summary>
/// The set of filters used by <see cref="Garage{T}.SearchByProperties"/>. It is both the filter itself and the single
/// place that knows which properties can be searched: each property has a name, a text parser and a match rule in
/// the private <c>Definitions</c> table. To make a new property searchable, add one entry to that table (and
/// optionally a typed accessor); the console parser, its help text and the garage filter pick it up automatically.
/// A property without a filter means "do not filter on this".
/// </summary>
public class SearchCriteria
{
    /// <summary>
    /// How one property is matched: its name and a match rule against a typed value.
    /// </summary>
    private sealed record Definition(string Name, Func<IVehicle, object, bool> Matches);

    private const string RegistrationKey = "registration";
    private const string ColorKey = "color";
    private const string WheelsKey = "wheels";
    private const string TypeKey = "type";
    private const string FuelKey = "fuel";
    private const string SizeKey = "size";

    private static readonly Definition[] Definitions =
    [
        new(RegistrationKey, (v, value) => string.Equals(v.RegistrationNumber, (string)value, StringComparison.OrdinalIgnoreCase)),
        new(ColorKey, (v, value) => string.Equals(v.Color, (string)value, StringComparison.OrdinalIgnoreCase)),
        new(WheelsKey, (v, value) => v.NumberOfWheels == (int)value),
        new(TypeKey, (v, value) => v.Type == (VehicleType)value),
        new(FuelKey, (v, value) => v is Car car && car.FuelType == (FuelType)value),
        new(SizeKey, (v, value) => ((IntRange)value).Contains(v.ParkingSize)),
    ];

    private readonly Dictionary<string, object> _filters = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Names of all searchable properties, in display order.</summary>
    public static IReadOnlyList<string> PropertyNames { get; } = Definitions.Select(d => d.Name).ToArray();

    /// <summary>Registration number to match, ignoring case.</summary>
    public string? RegistrationNumber
    {
        get => Get(RegistrationKey);
        set => Set(RegistrationKey, value);
    }

    /// <summary>Color to match, ignoring case.</summary>
    public string? Color
    {
        get => Get(ColorKey);
        set => Set(ColorKey, value);
    }

    /// <summary>Exact number of wheels to match.</summary>
    public int? NumberOfWheels
    {
        get => Get<int>(WheelsKey);
        set => Set(WheelsKey, value);
    }

    /// <summary>Vehicle type to match.</summary>
    public VehicleType? Type
    {
        get => Get<VehicleType>(TypeKey);
        set => Set(TypeKey, value);
    }

    /// <summary>Fuel type to match. Only cars have a fuel type, so other vehicles never match.</summary>
    public FuelType? FuelType
    {
        get => Get<FuelType>(FuelKey);
        set => Set(FuelKey, value);
    }

    /// <summary>Parking size to match, as an inclusive range. A single size is the range size-size.</summary>
    public IntRange? ParkingSize
    {
        get => Get<IntRange>(SizeKey);
        set => Set(SizeKey, value);
    }

    /// <summary>Returns true when the vehicle satisfies every filter that is set.</summary>
    public bool Matches(IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        return _filters.All(filter => Find(filter.Key)!.Matches(vehicle, filter.Value));
    }

    private static Definition? Find(string name) =>
        Array.Find(Definitions, d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));

    private T? Get<T>(string name) where T : struct =>
        _filters.TryGetValue(name, out object? value) ? (T)value : null;

    private string? Get(string name) =>
        _filters.TryGetValue(name, out object? value) ? (string)value : null;

    private void Set(string name, object? value)
    {
        if (value is null)
        {
            _filters.Remove(name);
        }
        else
        {
            _filters[name] = value;
        }
    }
}
