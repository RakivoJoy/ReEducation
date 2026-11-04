using System.Collections.Generic;

namespace SolutionSetup;

/// <summary>
/// Why a vehicle could not be parked.
/// </summary>
public enum ParkFailureReason
{
    /// <summary>A vehicle with the same registration number is already parked.</summary>
    Duplicate,

    /// <summary>The garage has no free space.</summary>
    Full,
}

/// <summary>
/// Outcome of parking a single vehicle.
/// </summary>
/// <param name="Parked">True if the vehicle was parked.</param>
/// <param name="Reason">The reason the vehicle was not parked; null on success.</param>
public sealed record ParkResult(bool Parked, ParkFailureReason? Reason)
{
    /// <summary>Creates a successful park result.</summary>
    public static ParkResult Success() => new(true, null);

    /// <summary>Creates a failed park result with the given reason.</summary>
    public static ParkResult Failure(ParkFailureReason reason) => new(false, reason);
}

/// <summary>
/// Outcome of removing a vehicle by registration number.
/// </summary>
/// <param name="Removed">True if a vehicle was removed.</param>
/// <param name="RegistrationNumber">The registration number that was searched for.</param>
public sealed record RemoveResult(bool Removed, string RegistrationNumber);

/// <summary>
/// Outcome of parking a batch of vehicles.
/// </summary>
/// <param name="Total">The number of vehicles attempted.</param>
/// <param name="Parked">The number of vehicles parked.</param>
/// <param name="Failures">Each vehicle that was not parked, with its registration number and reason.</param>
public sealed record PopulateResult(int Total, int Parked, IReadOnlyList<(string RegistrationNumber, ParkFailureReason Reason)> Failures);
