using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace SolutionSetup;

/// <summary>
/// An inclusive whole-number range used by numeric search filters. A single value such as "40" is the range 40-40.
/// </summary>
public readonly record struct IntRange(int Min, int Max)
{
    /// <summary>Returns true when <paramref name="value"/> lies inside the range, ends included.</summary>
    public bool Contains(int value) => value >= Min && value <= Max;

    /// <summary>
    /// Parses "40" or "10-30". Returns false for anything else, including a start larger than the end.
    /// Only non-negative numbers are supported, so the first '-' always separates the two ends.
    /// </summary>
    public static bool TryParse(string text, [NotNullWhen(true)] out IntRange? range)
    {
        range = null;
        string[] parts = text.Split('-');

        if (parts.Length == 1 && int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int single))
        {
            range = new IntRange(single, single);
        }
        else if (parts.Length == 2
            && int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int min)
            && int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int max)
            && min <= max)
        {
            range = new IntRange(min, max);
        }

        return range is not null;
    }

    public override string ToString() => Min == Max ? Min.ToString(CultureInfo.InvariantCulture) : $"{Min}-{Max}";
}
