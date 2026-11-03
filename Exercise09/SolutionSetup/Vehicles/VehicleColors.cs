using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SolutionSetup.Vehicles;

/// <summary>
/// The predefined colors a vehicle can have.
/// </summary>
public static class VehicleColors
{
    /// <summary>All allowed colors, ordered by how common they are.</summary>
    public static IReadOnlyList<string> All { get; } =
    [
        "Black", "White", "Silver", "Gray", "Red",
        "Blue", "Green", "Yellow", "Orange", "Brown",
        "Beige", "Gold", "Pink", "Purple", "Turquoise",
        "Teal", "Navy", "Maroon", "Bronze", "Burgundy"
    ];

    /// <summary>The five most common colors, offered to the user when parking a vehicle.</summary>
    public static IReadOnlyList<string> MostCommon { get; } = All.Take(5).ToArray();

    /// <summary>
    /// Returns the allowed color matching the given value, ignoring case and surrounding whitespace.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the color is null, empty, whitespace or not in <see cref="All"/>.</exception>
    public static string Normalize(string color)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(color);

        return TryNormalize(color, out string? match) ? match : throw new ArgumentException($"Color '{color}' is not allowed. Choose one of: {string.Join(", ", All)}.", nameof(color));
    }

    /// <summary>
    /// Tries to find the allowed color matching the given value, ignoring case and surrounding whitespace.
    /// </summary>
    public static bool TryNormalize(string? color, [NotNullWhen(true)] out string? normalized)
    {
        normalized = All.FirstOrDefault(allowed => string.Equals(allowed, color?.Trim(), StringComparison.OrdinalIgnoreCase));
        return normalized is not null;
    }
}
