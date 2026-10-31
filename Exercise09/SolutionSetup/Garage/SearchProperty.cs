using SolutionSetup.Vehicles;
using System;

namespace SolutionSetup;

/// <summary>
/// Describes one property that vehicles can be searched by. The search parser and the garage filter both use this
/// definition, so a new searchable property only needs a new entry in <see cref="SearchProperties"/>.
/// </summary>
/// <param name="Name">Canonical lowercase name used in search input, e.g. "color".</param>
/// <param name="Parse">Converts user text into a typed filter value, or returns null when the text is invalid.</param>
/// <param name="Matches">Returns true when a vehicle satisfies the typed filter value produced by <paramref name="Parse"/>.</param>
/// <param name="InvalidValueMessage">Builds the message shown when <paramref name="Parse"/> rejects the text.</param>
public sealed record SearchProperty(
    string Name,
    Func<string, object?> Parse,
    Func<IVehicle, object, bool> Matches,
    Func<string, string> InvalidValueMessage);
