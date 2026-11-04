using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SolutionSetup.Vehicles;

/// <summary>
/// Registration number rules: ABC123 or ABC12D, trimmed and uppercase.
/// </summary>
public static class RegistrationNumber
{
    private static readonly Regex Pattern = new("^[A-HJ-PR-UW-Z]{3}[0-9]{2}[0-9A-HJ-PR-UW-Z]{1}$");

    public const string Rule = "three letters (excluding I, Q, V) followed by 3 numbers or 2 numbers and 1 letter. ABC123 or ABC12D. ";

    /// <summary>
    /// Tries to normalize a registration number (trimmed and uppercase) that matches the ABC123 or ABC12D format.
    /// </summary>
    public static bool TryNormalize(string? registrationNumber, [NotNullWhen(true)] out string? normalized)
    {
        string candidate = registrationNumber?.Trim().ToUpperInvariant() ?? string.Empty;
        if (Pattern.IsMatch(candidate))
        {
            normalized = candidate;
            return true;
        }

        normalized = null;
        return false;
    }

    /// <summary>
    /// Normalizes a registration number or throws <see cref="ArgumentException"/> when it does not match the format.
    /// </summary>
    public static string NormalizeOrThrow(string registrationNumber)
    {
        if (!TryNormalize(registrationNumber, out string? normalized))
        {
            throw new ArgumentException($"Registration number '{registrationNumber}' must be {Rule}.", nameof(registrationNumber));
        }

        return normalized;
    }
}
