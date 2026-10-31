using SolutionSetup;
using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SolutionSetupTests
{
    // Placeholder tests for the planned IVehicle / generic / LINQ refactoring.
    // Not implemented yet: each test is skipped until its phase is started.

    public class GarageGenericTests
    {
        [Fact]
        public void ConcreteVehicles_AreAssignableToIVehicle()
        {
            IVehicle[] vehicles =
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Motorcycle("ABC124", "Pink", 3, 600),
                new Airplane("ABC125", "White", 3, 2),
                new Bus("ABC126", "Yellow", 6, 50),
                new Boat("ABC127", "White", 0, 12.5),
                new Truck("ABC128", "Blue", 6, 12000),
            };

            Assert.All(vehicles, vehicle => Assert.NotNull(vehicle));
        }

        [Fact]
        public void IVehicle_ExposesSharedProperties()
        {
            IVehicle vehicle = new Car("abc123", "Black", 4, FuelType.Gasoline);

            Assert.Equal("ABC123", vehicle.RegistrationNumber);
            Assert.Equal("Black", vehicle.Color);
            Assert.Equal(4, vehicle.NumberOfWheels);
            Assert.Equal(VehicleType.Car, vehicle.Type);
        }

        [Fact]
        public void GenericGarage_OfCar_AcceptsOnlyCars()
        {
            var garage = new Garage<Car>(2);

            Assert.True(garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline)).Success);
            Assert.All(garage.ListAllVehicles(), vehicle => Assert.IsType<Car>(vehicle));
        }

        [Fact]
        public void GenericGarage_OfIVehicle_AcceptsMixedTypes()
        {
            var garage = new Garage<IVehicle>(3);

            Assert.True(garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline)).Success);
            Assert.True(garage.AddVehicle(new Motorcycle("ABC124", "Pink", 3, 600)).Success);
            Assert.True(garage.AddVehicle(new Boat("ABC127", "White", 0, 12.5)).Success);
            Assert.Equal(3, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void GenericGarage_KeepsCapacityAndDuplicateRules()
        {
            var garage = new Garage<IVehicle>(1);

            Assert.True(garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline)).Success);
            Assert.False(garage.AddVehicle(new Car("abc123", "Red", 4, FuelType.Diesel)).Success);
            Assert.False(garage.AddVehicle(new Motorcycle("ABC124", "Pink", 3, 600)).Success);
            Assert.Equal(1, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void Enumerating_ReturnsParkedVehicles()
        {
            var garage = new Garage<IVehicle>(2);
            var car = new Car("ABC123", "Black", 4, FuelType.Gasoline);
            var motorcycle = new Motorcycle("ABC124", "Pink", 3, 600);
            garage.AddVehicle(car);
            garage.AddVehicle(motorcycle);

            Assert.Equal(new IVehicle[] { car, motorcycle }, garage.ToArray());
        }

        [Fact]
        public void ListAllVehicles_CannotMutateGarage()
        {
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            IReadOnlyList<IVehicle> vehicles = garage.ListAllVehicles();

            Assert.IsNotType<List<IVehicle>>(vehicles);
            Assert.Single(vehicles);
            Assert.Single(garage.ListAllVehicles());
        }

        [Fact]
        public void Populate_AcceptsLazyEnumerable()
        {
            var garage = new Garage<IVehicle>(2);

            IEnumerable<IVehicle> Generate()
            {
                yield return new Car("ABC123", "Black", 4, FuelType.Gasoline);
                yield return new Car("DEF456", "Red", 4, FuelType.Diesel);
            }

            var result = garage.Populate(Generate());

            Assert.True(result.Success);
            Assert.Equal(2, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void Populate_EmptySequence_ParksNothing()
        {
            var garage = new Garage<IVehicle>(2);

            var result = garage.Populate(Array.Empty<IVehicle>());

            Assert.True(result.Success);
            Assert.Equal("Parked 0 of 0 vehicles.", result.Message);
            Assert.Empty(garage.ListAllVehicles());
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

        [Fact]
        public void Constructor_RejectsLowercaseForbiddenLetter()
        {
            Assert.Throws<ArgumentException>(() => new Car("abv123", "Black", 4, FuelType.Gasoline));
        }
    }
}
