using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup;

/// <summary>
/// Registry of the properties vehicles can be searched by. Used by the search input parser, the search help text
/// and <see cref="Garage{T}.SearchByProperties"/>, so they cannot drift apart.
/// </summary>
public static class SearchProperties
{
    /// <summary>All searchable properties, in the order they are shown to the user.</summary>
    public static IReadOnlyList<SearchProperty> All { get; } =
    [
        new SearchProperty(
            "registration",
            text => text.Trim(),
            (vehicle, value) => string.Equals(vehicle.RegistrationNumber, (string)value, StringComparison.OrdinalIgnoreCase),
            text => $"Invalid registration '{text}'."),
        new SearchProperty(
            "color",
            text => text,
            (vehicle, value) => string.Equals(vehicle.Color, (string)value, StringComparison.OrdinalIgnoreCase),
            text => $"Invalid color '{text}'."),
        new SearchProperty(
            "wheels",
            text => int.TryParse(text, out int numberOfWheels) ? (object)numberOfWheels : null,
            (vehicle, value) => vehicle.NumberOfWheels == (int)value,
            text => $"Invalid number of wheels '{text}'. Please enter a whole number."),
        new SearchProperty(
            "type",
            text => Enum.GetValues<VehicleType>().Cast<object>().FirstOrDefault(type => string.Equals(type.ToString(), text, StringComparison.OrdinalIgnoreCase)),
            (vehicle, value) => vehicle.Type == (VehicleType)value,
            text => $"Invalid vehicle type '{text}'."),
    ];

    /// <summary>Finds a searchable property by name, ignoring case; returns null when no property has that name.</summary>
    public static SearchProperty? Find(string name) =>
        All.FirstOrDefault(property => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase));
}
