using SolutionSetup;
using SolutionSetup.Vehicles;
using Xunit;

namespace SolutionSetupTests
{
    // Placeholder tests for the planned IVehicle / generic / LINQ refactoring.
    // Not implemented yet: each test is skipped until its phase is started.

    public class GarageGenericTests
    {
        // TODO: Phase 1 - Car, Motorcycle, Airplane, Bus, Boat and Truck are all assignable to IVehicle.
        [Fact(Skip = "Not implemented: IVehicle does not exist yet.")]
        public void ConcreteVehicles_AreAssignableToIVehicle()
        {
        }

        // TODO: Phase 1 - IVehicle exposes RegistrationNumber, Color, NumberOfWheels and Type.
        [Fact(Skip = "Not implemented: IVehicle does not exist yet.")]
        public void IVehicle_ExposesSharedProperties()
        {
        }

        // TODO: Phase 2 - Garage<Car> accepts only cars.
        [Fact(Skip = "Not implemented: generic Garage<T> does not exist yet.")]
        public void GenericGarage_OfCar_AcceptsOnlyCars()
        {
        }

        // TODO: Phase 2 - Garage<IVehicle> accepts mixed vehicle types.
        [Fact(Skip = "Not implemented: generic Garage<T> does not exist yet.")]
        public void GenericGarage_OfIVehicle_AcceptsMixedTypes()
        {
        }

        // TODO: Phase 2 - capacity and duplicate-registration rules are unchanged for Garage<T>.
        [Fact(Skip = "Not implemented: generic Garage<T> does not exist yet.")]
        public void GenericGarage_KeepsCapacityAndDuplicateRules()
        {
        }

        // TODO: Phase 2 - enumerating Garage<T> returns the parked vehicles.
        [Fact(Skip = "Not implemented: Garage<T> does not implement IEnumerable<T> yet.")]
        public void Enumerating_ReturnsParkedVehicles()
        {
        }

        // TODO: Phase 2 - ListAllVehicles returns a read-only view; mutating it does not change the garage.
        [Fact(Skip = "Not implemented: IReadOnlyList<T> return type does not exist yet.")]
        public void ListAllVehicles_CannotMutateGarage()
        {
        }

        // TODO: Phase 2 - Populate accepts any IEnumerable<T>, including a lazy sequence.
        [Fact(Skip = "Not implemented: Populate(IEnumerable<T>) does not exist yet.")]
        public void Populate_AcceptsLazyEnumerable()
        {
        }

        // TODO: Phase 2 - Populate with an empty sequence parks nothing and reports "Parked 0 of 0 vehicles."
        [Fact(Skip = "Not implemented: Populate(IEnumerable<T>) does not exist yet.")]
        public void Populate_EmptySequence_ParksNothing()
        {
        }

        // TODO: Phase 3 - SearchByProperties with a single criterion matches only that criterion.
        [Fact(Skip = "Not implemented: LINQ search refactor not started.")]
        public void Search_SingleCriterion_MatchesOnlyThatCriterion()
        {
        }

        // TODO: Phase 3 - SearchByProperties with no criteria returns every parked vehicle.
        [Fact(Skip = "Not implemented: LINQ search refactor not started.")]
        public void Search_NoCriteria_ReturnsAllVehicles()
        {
        }

        // TODO: Phase 3 - combined criteria return only vehicles matching all of them.
        [Fact(Skip = "Not implemented: LINQ search refactor not started.")]
        public void Search_CombinedCriteria_MatchesAll()
        {
        }

        // TODO: Phase 3 - text search remains case-insensitive after the LINQ rewrite.
        [Fact(Skip = "Not implemented: LINQ search refactor not started.")]
        public void Search_TextFilters_AreCaseInsensitive()
        {
        }

        // TODO: Phase 3 - ListVehicleCountByType returns counts for mixed types; types with no vehicles are absent or zero.
        [Fact(Skip = "Not implemented: dictionary-based count not started.")]
        public void VehicleCountByType_CountsMixedTypes()
        {
        }

        // TODO: Phase 3 - an empty garage returns an empty count result.
        [Fact(Skip = "Not implemented: dictionary-based count not started.")]
        public void VehicleCountByType_EmptyGarage_ReturnsEmpty()
        {
        }

        // TODO: Phase 3 - removing a vehicle frees its space so a new vehicle can be parked.
        [Fact(Skip = "Not implemented: Remove via Remove(vehicle) not started.")]
        public void RemoveVehicle_FreesSpace()
        {
        }

        // TODO: Phase 3 - FindByRegistration returns the matching vehicle using FirstOrDefault.
        [Fact(Skip = "Not implemented: FirstOrDefault lookup not started.")]
        public void FindByRegistration_UsesFirstOrDefault()
        {
        }

        // TODO: Registration validation - lowercase input containing a forbidden letter is rejected (e.g. "abv123").
        [Fact(Skip = "Not implemented: add lowercase forbidden-letter case.")]
        public void Constructor_RejectsLowercaseForbiddenLetter()
        {
        }
    }
}
