using SolutionSetup.Vehicles;
using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Default <see cref="ISampleDataProvider"/> that returns the built-in <see cref="SampleData"/> vehicles.
/// </summary>
public sealed class SampleDataProvider : ISampleDataProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<IVehicle> CreateSampleVehicles() => SampleData.CreateSampleVehicles();
}
