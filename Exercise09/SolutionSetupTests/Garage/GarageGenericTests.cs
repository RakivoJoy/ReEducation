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

        private static Garage<IVehicle> MixedGarage()
        {
            var garage = new Garage<IVehicle>(5);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            garage.AddVehicle(new Car("DEF456", "Red", 4, FuelType.Diesel));
            garage.AddVehicle(new Motorcycle("GHL789", "Black", 2, 600));
            garage.AddVehicle(new Bus("JKL012", "Yellow", 6, 50));
            return garage;
        }

        [Fact]
        public void Search_SingleCriterion_MatchesOnlyThatCriterion()
        {
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Black" });

            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.Equal("Black", v.Color));
        }

        [Fact]
        public void Search_NoCriteria_ReturnsAllVehicles()
        {
            var result = MixedGarage().SearchByProperties(new SearchCriteria());

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void Search_CombinedCriteria_MatchesAll()
        {
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Black", Type = VehicleType.Car, NumberOfWheels = 4 });

            var vehicle = Assert.Single(result);
            Assert.Equal("ABC123", vehicle.RegistrationNumber);
        }

        [Fact]
        public void Search_CombinedCriteria_NoneMatch_ReturnsEmpty()
        {
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Red", Type = VehicleType.Bus });

            Assert.Empty(result);
        }

        [Fact]
        public void Search_ByWheelsAndType_Matches()
        {
            var result = MixedGarage().SearchByProperties(new SearchCriteria { NumberOfWheels = 2, Type = VehicleType.Motorcycle });

            Assert.Equal("GHL789", Assert.Single(result).RegistrationNumber);
        }

        [Fact]
        public void Search_TextFilters_AreCaseInsensitive()
        {
            var garage = MixedGarage();

            Assert.Equal(2, garage.SearchByProperties(new SearchCriteria { Color = "bLaCk" }).Count);
            Assert.Single(garage.SearchByProperties(new SearchCriteria { RegistrationNumber = "def456" }));
        }

        [Fact]
        public void Search_ReturnsSnapshot_NotAffectedByLaterRemoval()
        {
            var garage = MixedGarage();
            var result = garage.SearchByProperties(new SearchCriteria());

            garage.RemoveVehicleByRegistration("ABC123");

            Assert.Equal(4, result.Count);
            Assert.Equal(3, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void VehicleCountByType_CountsMixedTypes()
        {
            var counts = MixedGarage().ListVehicleCountByType();

            Assert.Equal(2, counts[VehicleType.Car]);
            Assert.Equal(1, counts[VehicleType.Motorcycle]);
            Assert.Equal(1, counts[VehicleType.Bus]);
            Assert.False(counts.ContainsKey(VehicleType.Boat));
            Assert.Equal(4, counts.Values.Sum());
        }

        [Fact]
        public void VehicleCountByType_EmptyGarage_ReturnsEmpty()
        {
            Assert.Empty(new Garage<IVehicle>(2).ListVehicleCountByType());
        }

        [Fact]
        public void VehicleCountByType_AfterRemoval_DropsType()
        {
            var garage = MixedGarage();
            garage.RemoveVehicleByRegistration("JKL012");

            Assert.False(garage.ListVehicleCountByType().ContainsKey(VehicleType.Bus));
        }

        [Fact]
        public void RemoveVehicle_FreesSpace()
        {
            var garage = new Garage<IVehicle>(1);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            Assert.True(garage.RemoveVehicleByRegistration("abc123").Success);
            Assert.True(garage.AddVehicle(new Car("DEF456", "Red", 4, FuelType.Diesel)).Success);
        }

        [Fact]
        public void RemoveVehicle_Unknown_Fails_AndKeepsOthers()
        {
            var garage = MixedGarage();

            Assert.False(garage.RemoveVehicleByRegistration("ZZZ999").Success);
            Assert.Equal(4, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void RemoveVehicle_RemovesOnlyTheMatch()
        {
            var garage = MixedGarage();

            garage.RemoveVehicleByRegistration(" def456 ");

            Assert.Null(garage.FindByRegistration("DEF456"));
            Assert.NotNull(garage.FindByRegistration("ABC123"));
        }

        [Fact]
        public void FindByRegistration_UsesFirstOrDefault()
        {
            var garage = MixedGarage();

            Assert.Equal("GHL789", garage.FindByRegistration(" ghl789 ")!.RegistrationNumber);
            Assert.Null(garage.FindByRegistration("ZZZ999"));
        }

        [Fact]
        public void Populate_PartialFailure_ReportsCounts()
        {
            var garage = new Garage<IVehicle>(2);

            var result = garage.Populate(new IVehicle[]
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Car("abc123", "Red", 4, FuelType.Diesel),
                new Car("DEF456", "Red", 4, FuelType.Diesel),
                new Car("GHL789", "Red", 4, FuelType.Diesel),
            });

            Assert.False(result.Success);
            Assert.Equal("Parked 2 of 4 vehicles.", result.Message);
        }

        [Fact]
        public void Populate_LazySequence_IsEnumeratedOnce()
        {
            var garage = new Garage<IVehicle>(3);
            int enumerations = 0;

            IEnumerable<IVehicle> Generate()
            {
                enumerations++;
                yield return new Car("ABC123", "Black", 4, FuelType.Gasoline);
            }

            garage.Populate(Generate());

            Assert.Equal(1, enumerations);
        }

        [Fact]
        public void Constructor_RejectsLowercaseForbiddenLetter()
        {
            Assert.Throws<ArgumentException>(() => new Car("abv123", "Black", 4, FuelType.Gasoline));
        }
    }
}
