using System;
using System.Text.RegularExpressions;

namespace SolutionSetup.Vehicles;

/// <summary>
/// Base class for all vehicles that can be parked in a garage.
/// </summary>
public abstract class Vehicle
{
    private static readonly Regex RegistrationNumberPattern = new("^[A-Z]{3}[0-9]{3}$");

    /// <summary>Unique registration number in the format ABC123. Set once and stored trimmed and uppercase.</summary>
    public string RegistrationNumber { get; }

    private string _color = string.Empty;

    /// <summary>Color of the vehicle. Must be one of the colors in <see cref="VehicleColors.All"/>.</summary>
    public string Color
    {
        get => _color;
        set => _color = VehicleColors.Normalize(value);
    }

    /// <summary>Number of wheels on the vehicle. Cannot be negative.</summary>
    public int NumberOfWheels { get; set; }

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

        string normalizedRegistrationNumber = registrationNumber.Trim().ToUpperInvariant();
        if (!RegistrationNumberPattern.IsMatch(normalizedRegistrationNumber))
        {
            throw new ArgumentException($"Registration number '{registrationNumber}' must be in the format ABC123.", nameof(registrationNumber));
        }

        RegistrationNumber = normalizedRegistrationNumber;
        Color = color;
        NumberOfWheels = numberOfWheels;
    }

    /// <summary>
    /// Extra details shown by <see cref="ToString"/>. Subclasses override this to add their own information.
    /// </summary>
    protected virtual string Details => string.Empty;

    /// <summary>
    /// Returns a one-line description of the vehicle, including optional subclass details.
    /// </summary>
    public override string ToString()
    {
        string summary = $"{Type} {RegistrationNumber}, {Color}, {NumberOfWheels} wheels";
        return string.IsNullOrEmpty(Details) ? summary : $"{summary}, {Details}";
    }
}
