using System;
using System.IO;
using System.Threading.Tasks;
using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using Xunit;

namespace SolutionSetupTests;

[Collection(FileIOCollection.Name)]
public class GarageRoundTripTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public GarageRoundTripTests() => Directory.CreateDirectory(_folder);

    private string FilePath => Path.Combine(_folder, "garage.json");

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    [Fact]
    public async Task SaveThenLoad_PreservesCapacityAndAllVehicleTypes()
    {
        // Arrange
        Garage<IVehicle> original = new(10);
        original.AddVehicle(new Car("CAR123", "Black", 4, FuelType.Diesel, 5));
        original.AddVehicle(new Bus("BUS123", "Blue", 6, 40, 20));
        original.AddVehicle(new Airplane("ABC123", "White", 3, 2, 60));
        original.AddVehicle(new Truck("TRK123", "Red", 8, 5000, 25));
        original.AddVehicle(new Motorcycle("MOT123", "Green", 2, 600, 5));
        original.AddVehicle(new Boat("BOA123", "Silver", 0, 12.5, 15));
        JsonGarageStorage storage = new(FilePath);

        // Act
        OperationResult saveResult = await storage.SaveAsync(original);
        GarageLoadResult loadResult = await storage.LoadAsync();

        // Assert
        Assert.True(saveResult.Success);
        Assert.True(loadResult.Success);
        Assert.NotNull(loadResult.Garage);
        Garage<IVehicle> loaded = loadResult.Garage!;
        Assert.Equal(original.Capacity, loaded.Capacity);
        Assert.Equal(original.ListAllVehicles().Count, loaded.ListAllVehicles().Count);

        Car car = Assert.IsType<Car>(loaded.FindByRegistration("CAR123"));
        Assert.Equal(FuelType.Diesel, car.FuelType);
        Assert.Equal(5, car.ParkingSize);

        Bus bus = Assert.IsType<Bus>(loaded.FindByRegistration("BUS123"));
        Assert.Equal(40, bus.NumberOfSeats);
        Assert.Equal(20, bus.ParkingSize);

        Airplane airplane = Assert.IsType<Airplane>(loaded.FindByRegistration("ABC123"));
        Assert.Equal(2, airplane.NumberOfEngines);
        Assert.Equal(60, airplane.ParkingSize);

        Truck truck = Assert.IsType<Truck>(loaded.FindByRegistration("TRK123"));
        Assert.Equal(5000, truck.LoadCapacity);
        Assert.Equal(25, truck.ParkingSize);

        Motorcycle motorcycle = Assert.IsType<Motorcycle>(loaded.FindByRegistration("MOT123"));
        Assert.Equal(600, motorcycle.CylinderVolume);
        Assert.Equal(5, motorcycle.ParkingSize);

        Boat boat = Assert.IsType<Boat>(loaded.FindByRegistration("BOA123"));
        Assert.Equal(12.5, boat.Length);
        Assert.Equal(15, boat.ParkingSize);
    }
}
