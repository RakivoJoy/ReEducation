using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using System.Threading;
using System.Threading.Tasks;

namespace SolutionSetupTests;

internal sealed class InMemoryGarageStorage : IGarageStorage
{
    private Garage<IVehicle>? _saved;

    public Task<OperationResult> SaveAsync(Garage<IVehicle> garage, CancellationToken cancellationToken = default)
    {
        _saved = garage;
        return Task.FromResult(OperationResult.Ok("saved in memory"));
    }

    public Task<GarageLoadResult> LoadAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_saved is null
            ? GarageLoadResult.Fail("nothing saved in memory")
            : GarageLoadResult.Ok(_saved, "loaded from memory"));
    }
}
