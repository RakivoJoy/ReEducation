namespace SolutionSetup;

/// <summary>
/// Sits between a user interface and the garage. It holds the garage and exposes one method per menu option,
/// so a UI never touches the garage directly and can be tested without Console input or output.
/// Consumers that need only part of the behavior can depend on <see cref="IGarageLifecycle"/>,
/// <see cref="IVehicleOperations"/>, <see cref="IVehicleQueries"/> or <see cref="IGaragePersistence"/>.
/// </summary>
public interface IHandler : IGarageLifecycle, IVehicleOperations, IVehicleQueries, IGaragePersistence
{
}
