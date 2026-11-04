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
            // Arrange
            IVehicle[] vehicles =
            [
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Motorcycle("ABC124", "Pink", 3, 600),
                new Airplane("ABC125", "White", 3, 2),
                new Bus("ABC126", "Yellow", 6, 50),
                new Boat("ABC127", "White", 0, 12.5),
                new Truck("ABC128", "Blue", 6, 12000),
            ];

            // Act & Assert
            Assert.All(vehicles, vehicle => Assert.NotNull(vehicle));
        }

        [Fact]
        public void IVehicle_ExposesSharedProperties()
        {
            // Arrange
            IVehicle vehicle = new Car("abc123", "Black", 4, FuelType.Gasoline);

            // Act & Assert
            Assert.Equal("ABC123", vehicle.RegistrationNumber);
            Assert.Equal("Black", vehicle.Color);
            Assert.Equal(4, vehicle.NumberOfWheels);
            Assert.Equal(VehicleType.Car, vehicle.Type);
        }

        [Fact]
        public void GenericGarage_OfCar_AcceptsOnlyCars()
        {
            // Arrange
            var garage = new Garage<Car>(2);

            // Act
            var added = garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            // Assert
            Assert.True(added.Parked);
            Assert.All(garage.ListAllVehicles(), vehicle => Assert.IsType<Car>(vehicle));
        }

        [Fact]
        public void GenericGarage_OfIVehicle_AcceptsMixedTypes()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);

            // Act
            var car = garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            var motorcycle = garage.AddVehicle(new Motorcycle("ABC124", "Pink", 3, 600));
            var boat = garage.AddVehicle(new Boat("ABC127", "White", 0, 12.5));

            // Assert
            Assert.True(car.Parked);
            Assert.True(motorcycle.Parked);
            Assert.True(boat.Parked);
            Assert.Equal(3, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void GenericGarage_KeepsCapacityAndDuplicateRules()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);

            // Act
            var first = garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            var duplicate = garage.AddVehicle(new Car("abc123", "Red", 4, FuelType.Diesel));
            var full = garage.AddVehicle(new Motorcycle("ABC124", "Pink", 3, 600));

            // Assert
            Assert.True(first.Parked);
            Assert.False(duplicate.Parked);
            Assert.False(full.Parked);
            Assert.Single(garage.ListAllVehicles());
        }

        [Fact]
        public void Enumerating_ReturnsParkedVehicles()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            var car = new Car("ABC123", "Black", 4, FuelType.Gasoline);
            var motorcycle = new Motorcycle("ABC124", "Pink", 3, 600);
            garage.AddVehicle(car);
            garage.AddVehicle(motorcycle);

            // Act & Assert
            Assert.Equal([car, motorcycle], garage.ToArray());
        }

        [Fact]
        public void ListAllVehicles_CannotMutateGarage()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            // Act
            IReadOnlyList<IVehicle> vehicles = garage.ListAllVehicles();

            // Assert
            Assert.IsNotType<List<IVehicle>>(vehicles);
            Assert.Single(vehicles);
            Assert.Single(garage.ListAllVehicles());
        }

        [Fact]
        public void Populate_AcceptsLazyEnumerable()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            static IEnumerable<IVehicle> Generate()
            {
                yield return new Car("ABC123", "Black", 4, FuelType.Gasoline);
                yield return new Car("DEF456", "Red", 4, FuelType.Diesel);
            }

            // Act
            var result = garage.Populate(Generate());

            // Assert
            Assert.Empty(result.Failures);
            Assert.Equal(2, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void Populate_EmptySequence_ParksNothing()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act
            var result = garage.Populate(Array.Empty<IVehicle>());

            // Assert
            Assert.Equal(0, result.Total);
            Assert.Equal(0, result.Parked);
            Assert.Empty(result.Failures);
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
            // Arrange & Act
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Black" });

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.Equal("Black", v.Color));
        }

        [Fact]
        public void Search_NoCriteria_ReturnsAllVehicles()
        {
            // Arrange & Act
            var result = MixedGarage().SearchByProperties(new SearchCriteria());

            // Assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void Search_CombinedCriteria_MatchesAll()
        {
            // Arrange & Act
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Black", Type = VehicleType.Car, NumberOfWheels = 4 });

            // Assert
            var vehicle = Assert.Single(result);
            Assert.Equal("ABC123", vehicle.RegistrationNumber);
        }

        [Fact]
        public void Search_CombinedCriteria_NoneMatch_ReturnsEmpty()
        {
            // Arrange & Act
            var result = MixedGarage().SearchByProperties(new SearchCriteria { Color = "Red", Type = VehicleType.Bus });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Search_ByWheelsAndType_Matches()
        {
            // Arrange & Act
            var result = MixedGarage().SearchByProperties(new SearchCriteria { NumberOfWheels = 2, Type = VehicleType.Motorcycle });

            // Assert
            Assert.Equal("GHL789", Assert.Single(result).RegistrationNumber);
        }

        [Fact]
        public void Search_TextFilters_AreCaseInsensitive()
        {
            // Arrange
            var garage = MixedGarage();

            // Act & Assert
            Assert.Equal(2, garage.SearchByProperties(new SearchCriteria { Color = "bLaCk" }).Count);
            Assert.Single(garage.SearchByProperties(new SearchCriteria { RegistrationNumber = "def456" }));
        }

        [Fact]
        public void Search_ReturnsSnapshot_NotAffectedByLaterRemoval()
        {
            // Arrange
            var garage = MixedGarage();
            var result = garage.SearchByProperties(new SearchCriteria());

            // Act
            garage.RemoveVehicleByRegistration("ABC123");

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal(3, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void VehicleCountByType_CountsMixedTypes()
        {
            // Arrange & Act
            var counts = MixedGarage().ListVehicleCountByType();

            // Assert
            Assert.Equal(2, counts[VehicleType.Car]);
            Assert.Equal(1, counts[VehicleType.Motorcycle]);
            Assert.Equal(1, counts[VehicleType.Bus]);
            Assert.False(counts.ContainsKey(VehicleType.Boat));
            Assert.Equal(4, counts.Values.Sum());
        }

        [Fact]
        public void VehicleCountByType_EmptyGarage_ReturnsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Empty(new Garage<IVehicle>(2).ListVehicleCountByType());
        }

        [Fact]
        public void VehicleCountByType_AfterRemoval_DropsType()
        {
            // Arrange
            var garage = MixedGarage();

            // Act
            garage.RemoveVehicleByRegistration("JKL012");

            // Assert
            Assert.False(garage.ListVehicleCountByType().ContainsKey(VehicleType.Bus));
        }

        [Fact]
        public void RemoveVehicle_FreesSpace()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            // Act
            var removed = garage.RemoveVehicleByRegistration("abc123");

            // Assert
            Assert.True(removed.Removed);
            Assert.True(garage.AddVehicle(new Car("DEF456", "Red", 4, FuelType.Diesel)).Parked);
        }

        [Fact]
        public void RemoveVehicle_Unknown_Fails_AndKeepsOthers()
        {
            // Arrange
            var garage = MixedGarage();

            // Act & Assert
            Assert.False(garage.RemoveVehicleByRegistration("ZZZ999").Removed);
            Assert.Equal(4, garage.ListAllVehicles().Count);
        }

        [Fact]
        public void RemoveVehicle_RemovesOnlyTheMatch()
        {
            // Arrange
            var garage = MixedGarage();

            // Act
            garage.RemoveVehicleByRegistration(" def456 ");

            // Assert
            Assert.Null(garage.FindByRegistration("DEF456"));
            Assert.NotNull(garage.FindByRegistration("ABC123"));
        }

        [Fact]
        public void FindByRegistration_UsesFirstOrDefault()
        {
            // Arrange
            var garage = MixedGarage();

            // Act & Assert
            Assert.Equal("GHL789", garage.FindByRegistration(" ghl789 ")!.RegistrationNumber);
            Assert.Null(garage.FindByRegistration("ZZZ999"));
        }

        [Fact]
        public void Populate_PartialFailure_ReportsCounts()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);

            // Act
            var result = garage.Populate(new IVehicle[]
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Car("abc123", "Red", 4, FuelType.Diesel),
                new Car("DEF456", "Red", 4, FuelType.Diesel),
                new Car("GHL789", "Red", 4, FuelType.Diesel),
            });

            // Assert
            Assert.Equal(2, result.Parked);
            Assert.Equal(4, result.Total);
        }

        [Fact]
        public void Populate_LazySequence_IsEnumeratedOnce()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            int enumerations = 0;

            IEnumerable<IVehicle> Generate()
            {
                enumerations++;
                yield return new Car("ABC123", "Black", 4, FuelType.Gasoline);
            }

            // Act
            garage.Populate(Generate());

            // Assert
            Assert.Equal(1, enumerations);
        }

        [Fact]
        public void Constructor_RejectsLowercaseForbiddenLetter()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("abv123", "Black", 4, FuelType.Gasoline));
        }
    }
}
