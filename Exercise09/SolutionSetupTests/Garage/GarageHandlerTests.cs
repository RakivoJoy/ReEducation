using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GarageHandlerTests
    {
        [Fact]
        public void HasGarage_IsFalse_BeforeGarageCreated()
        {
            // Arrange
            var handler = new GarageHandler();

            // Act
            var hasGarage = handler.HasGarage;

            // Assert
            Assert.False(hasGarage);
        }

        [Fact]
        public void CreateGarage_CreatesGarage_WhenCapacityValid()
        {
            // Arrange
            var handler = new GarageHandler();

            // Act
            var result = handler.CreateGarage(3);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("capacity 3", result.Message);
            Assert.True(handler.HasGarage);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateGarage_Fails_WhenCapacityNotPositive(int capacity)
        {
            // Arrange
            var handler = new GarageHandler();

            // Act
            var result = handler.CreateGarage(capacity);

            // Assert
            Assert.False(result.Success);
            Assert.False(handler.HasGarage);
        }

        [Fact]
        public void CreateGarage_ReplacesExistingGarage()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(3);
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            handler.CreateGarage(3);

            // Assert
            Assert.Empty(handler.ListVehicles());
        }

        [Fact]
        public void ParkVehicle_ParksVehicle_AndFindVehicleReturnsIt()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(2);

            // Act
            var result = handler.ParkVehicle(CreateCar("ABC123"));
            var found = handler.FindVehicle("abc123");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(found);
        }

        [Fact]
        public void ParkVehicle_Fails_WhenGarageIsFull()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(1);
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            var result = handler.ParkVehicle(CreateCar("ABC124"));

            // Assert
            Assert.False(result.Success);
            Assert.Contains("full", result.Message);
        }

        [Fact]
        public void RemoveVehicle_RemovesParkedVehicle()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(2);
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            var result = handler.RemoveVehicle("ABC123");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(handler.ListVehicles());
        }

        [Fact]
        public void PopulateGarage_ParksSampleVehicles()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(3);

            // Act
            var result = handler.PopulateGarage();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(3, handler.ListVehicles().Count);
        }

        [Fact]
        public void PopulateGarage_ReportsReasons_WhenUsedTwice()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(10);
            handler.PopulateGarage();

            // Act
            var result = handler.PopulateGarage();

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Parked 0 of 3", result.Message);
            Assert.Contains("already parked", result.Message);
        }

        [Fact]
        public void ListVehicleCounts_CountsVehiclesPerType()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(3);
            handler.ParkVehicle(CreateCar("ABC123"));
            handler.ParkVehicle(CreateCar("ABC124"));

            // Act
            var counts = handler.ListVehicleCounts();

            // Assert
            Assert.Equal(2, counts[VehicleType.Car]);
        }

        [Fact]
        public void SearchVehicles_ReturnsMatchingVehicles()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(3);
            handler.ParkVehicle(new Truck("TRK001", "Blue", 6, 9000));
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            var results = handler.SearchVehicles(new SearchCriteria { Type = VehicleType.Truck });

            // Assert
            Assert.Single(results);
        }

        [Fact]
        public void FindVehicle_ReturnsNull_WhenNotParked()
        {
            // Arrange
            var handler = new GarageHandler();
            handler.CreateGarage(1);

            // Act
            var found = handler.FindVehicle("ABC123");

            // Assert
            Assert.Null(found);
        }

        [Fact]
        public void Operations_Throw_WhenNoGarageCreated()
        {
            // Arrange
            var handler = new GarageHandler();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => handler.PopulateGarage());
            Assert.Throws<InvalidOperationException>(() => handler.ParkVehicle(CreateCar("ABC123")));
            Assert.Throws<InvalidOperationException>(() => handler.RemoveVehicle("ABC123"));
            Assert.Throws<InvalidOperationException>(() => handler.ListVehicles());
            Assert.Throws<InvalidOperationException>(() => handler.ListVehicleCounts());
            Assert.Throws<InvalidOperationException>(() => handler.FindVehicle("ABC123"));
            Assert.Throws<InvalidOperationException>(() => handler.SearchVehicles(new SearchCriteria()));
        }

        [Fact]
        public void SaveGarage_ThrowsInvalidOperation_WhenNoGarageCreated()
        {
            // Arrange
            var storage = new FakeGarageStorage();
            var handler = new GarageHandler(storage);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => handler.SaveGarage());
            Assert.Equal(0, storage.SaveCount);
        }

        [Fact]
        public void SaveGarage_PassesCurrentGarageToStorage()
        {
            // Arrange
            var storage = new FakeGarageStorage();
            var handler = new GarageHandler(storage);
            handler.CreateGarage(4);
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            OperationResult result = handler.SaveGarage();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(1, storage.SaveCount);
            Assert.Single(storage.SavedGarage!.ListAllVehicles());
        }

        [Fact]
        public void SaveGarage_ReturnsStorageFailure()
        {
            // Arrange
            var storage = new FakeGarageStorage { SaveResult = OperationResult.Fail("disk full") };
            var handler = new GarageHandler(storage);
            handler.CreateGarage(4);

            // Act
            OperationResult result = handler.SaveGarage();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("disk full", result.Message);
        }

        [Fact]
        public void LoadGarage_ReplacesCurrentGarage_WhenLoadSucceeds()
        {
            // Arrange
            var loaded = new Garage<IVehicle>(9);
            loaded.AddVehicle(CreateCar("XYZ789"));
            var storage = new FakeGarageStorage { LoadResult = GarageLoadResult.Ok(loaded, "loaded ok") };
            var handler = new GarageHandler(storage);
            handler.CreateGarage(2);

            // Act
            OperationResult result = handler.LoadGarage();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("loaded ok", result.Message);
            Assert.NotNull(handler.FindVehicle("XYZ789"));
        }

        [Fact]
        public void LoadGarage_KeepsCurrentGarage_WhenLoadFails()
        {
            // Arrange
            var storage = new FakeGarageStorage { LoadResult = GarageLoadResult.Fail("file missing") };
            var handler = new GarageHandler(storage);
            handler.CreateGarage(2);
            handler.ParkVehicle(CreateCar("ABC123"));

            // Act
            OperationResult result = handler.LoadGarage();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("file missing", result.Message);
            Assert.True(handler.HasGarage);
            Assert.NotNull(handler.FindVehicle("ABC123"));
        }

        [Fact]
        public void LoadGarage_LeavesNoGarage_WhenLoadFailsAndNoneExisted()
        {
            // Arrange
            var storage = new FakeGarageStorage { LoadResult = GarageLoadResult.Fail("invalid JSON") };
            var handler = new GarageHandler(storage);

            // Act
            OperationResult result = handler.LoadGarage();

            // Assert
            Assert.False(result.Success);
            Assert.False(handler.HasGarage);
        }

        private sealed class FakeGarageStorage : IGarageStorage
        {
            public int SaveCount { get; private set; }
            public Garage<IVehicle>? SavedGarage { get; private set; }
            public OperationResult SaveResult { get; init; } = OperationResult.Ok("saved");
            public GarageLoadResult LoadResult { get; init; } = GarageLoadResult.Fail("no load configured");

            public Task<OperationResult> SaveAsync(Garage<IVehicle> garage, CancellationToken cancellationToken = default)
            {
                SaveCount++;
                SavedGarage = garage;
                return Task.FromResult(SaveResult);
            }

            public Task<GarageLoadResult> LoadAsync(CancellationToken cancellationToken = default)
                => Task.FromResult(LoadResult);
        }
    }
}
