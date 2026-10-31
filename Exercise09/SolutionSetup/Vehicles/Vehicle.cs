using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SolutionSetup.Vehicles;

/// <summary>
/// Base class for all vehicles that can be parked in a garage.
/// </summary>
public abstract class Vehicle : IVehicle
{
    private static readonly Regex RegistrationNumberPattern = new("^[A-HJ-PR-UW-Z]{3}[0-9]{2}[0-9A-HJ-PR-UW-Z]{1}$");

    /// <summary>Unique registration number in ABC123 or ABC12D. Excluding I, Q, V. Set once and stored trimmed and uppercase.</summary>
    public string RegistrationNumber { get; }

    /// <summary>Color of the vehicle. Must be one of the colors in <see cref="VehicleColors.All"/>. Set once at construction.</summary>
    // Design decision: colors are limited to the fixed list in VehicleColors. This keeps the data consistent
    // ("grey" and "gray" cannot both exist) and makes searching by color reliable. To allow more colors, add them to VehicleColors.All.
    public string Color { get; }

    /// <summary>Number of wheels on the vehicle. Cannot be negative. Set once at construction.</summary>
    public int NumberOfWheels { get; }

    /// <summary>The vehicle type, implemented by each subclass.</summary>
    public abstract VehicleType Type { get; }

    /// <summary>
    /// Creates a vehicle with the shared properties.
    /// </summary>
    /// <param name="registrationNumber">Registration number in the format ABC123 (three letters followed by three digits). Must not be null, empty or whitespace.</param>
    /// <param name="color">Color. Must be one of the colors in <see cref="VehicleColors.All"/>.</param>
    /// <param name="numberOfWheels">Number of wheels. Must not be negative.</param>
    /// <exception cref="ArgumentException">Thrown when the registration number is null, empty or whitespace or not in the format ABC123, or when the color is not an allowed color.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="numberOfWheels"/> is negative.</exception>
    protected Vehicle(string registrationNumber, string color, int numberOfWheels)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(registrationNumber);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfWheels);

        if (!TryNormalizeRegistrationNumber(registrationNumber, out string? normalizedRegistrationNumber))
        {
            throw new ArgumentException($"Registration number '{registrationNumber}' must be three letters (excluding I, Q, V) followed by three alphanumerics.", nameof(registrationNumber));
        }

        RegistrationNumber = normalizedRegistrationNumber;
        Color = VehicleColors.Normalize(color);
        NumberOfWheels = numberOfWheels;
    }

    /// <summary>
    /// Tries to normalize a registration number (trimmed and uppercase) that matches the ABC123 or ABC12D format.
    /// </summary>
    public static bool TryNormalizeRegistrationNumber(string? registrationNumber, [NotNullWhen(true)] out string? normalized)
    {
        string candidate = registrationNumber?.Trim().ToUpperInvariant() ?? string.Empty;
        if (RegistrationNumberPattern.IsMatch(candidate))
        {
            normalized = candidate;
            return true;
        }

        normalized = null;
        return false;
    }

    /// <summary>
    /// Extra details shown by <see cref="ToString"/>. Subclasses override this to add their own information.
    /// </summary>
    protected virtual string Details => string.Empty;

    /// <summary>
    /// Throws if the value is outside the inclusive range (NaN is also rejected). The message names the valid range and unit.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the range.</exception>
    protected static void ThrowIfOutOfRange(double value, double min, double max, string description, string unit, string paramName)
    {
        if (!(value >= min && value <= max))
        {
            string suffix = string.IsNullOrEmpty(unit) ? string.Empty : $" {unit}";
            throw new ArgumentOutOfRangeException(paramName, value, $"{description} must be between {min}{suffix} and {max}{suffix}.");
        }
    }

    /// <summary>
    /// Returns a one-line description of the vehicle, including optional subclass details.
    /// </summary>
    public override string ToString()
    {
        string summary = $"{Type} {RegistrationNumber}, {Color}, {NumberOfWheels} wheels";
        return string.IsNullOrEmpty(Details) ? summary : $"{summary}, {Details}";
    }
}
