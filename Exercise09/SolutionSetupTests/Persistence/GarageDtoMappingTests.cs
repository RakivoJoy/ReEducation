using System;
using System.Collections.Generic;
using System.IO;
using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.Vehicles;
using Xunit;

namespace SolutionSetupTests;

public class GarageDtoMappingTests
{
    private static Garage<IVehicle> CreateGarageWithOneOfEachType()
    {
        Garage<IVehicle> garage = new(10);
        garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Diesel, 5));
        garage.AddVehicle(new Bus("BUS001", "Red", 6, 40));
        garage.AddVehicle(new Airplane("PLN001", "White", 3, 2));
        garage.AddVehicle(new Truck("TRK001", "Blue", 6, 5000));
        garage.AddVehicle(new Motorcycle("MOT001", "Black", 2, 600));
        garage.AddVehicle(new Boat("BOT001", "White", 0, 12.5));
        return garage;
    }

    [Fact]
    public void ToDto_SetsCurrentVersionAndCapacity()
    {
        GarageDto dto = GarageMapper.ToDto(new Garage<IVehicle>(7));

        Assert.Equal(GarageMapper.CurrentVersion, dto.Version);
        Assert.Equal(7, dto.Capacity);
        Assert.Empty(dto.Vehicles);
    }

    [Fact]
    public void ToDto_ThenToGarage_RoundTripsEachVehicleType()
    {
        GarageDto original = GarageMapper.ToDto(CreateGarageWithOneOfEachType());

        IGarage<IVehicle> restored = GarageMapper.ToGarage(original);
        GarageDto restoredDto = GarageMapper.ToDto(restored);

        Assert.Equal(original.Capacity, restoredDto.Capacity);
        Assert.Equal(original.Vehicles, restoredDto.Vehicles);
    }

    [Fact]
    public void ToDto_CopiesTypeSpecificFields()
    {
        GarageDto dto = GarageMapper.ToDto(CreateGarageWithOneOfEachType());

        Assert.Equal(FuelType.Diesel, dto.Vehicles[0].FuelType);
        Assert.Equal(40, dto.Vehicles[1].NumberOfSeats);
        Assert.Equal(2, dto.Vehicles[2].NumberOfEngines);
        Assert.Equal(5000, dto.Vehicles[3].LoadCapacity);
        Assert.Equal(600, dto.Vehicles[4].CylinderVolume);
        Assert.Equal(12.5, dto.Vehicles[5].Length);
    }

    [Fact]
    public void ToGarage_UnsupportedVersion_Throws()
    {
        GarageDto dto = new(GarageMapper.CurrentVersion + 1, 5, []);

        Assert.Throws<InvalidDataException>(() => GarageMapper.ToGarage(dto));
    }

    [Fact]
    public void ToGarage_MissingTypeSpecificField_Throws()
    {
        VehicleDto car = new(VehicleType.Car, "ABC123", "Black", 4, 5);
        GarageDto dto = new(GarageMapper.CurrentVersion, 5, [car]);

        ArgumentException ex = Assert.Throws<ArgumentException>(() => GarageMapper.ToGarage(dto));
        Assert.Equal("FuelType", ex.ParamName);
    }

    [Fact]
    public void ToGarage_InvalidVehicleData_Throws()
    {
        VehicleDto car = new(VehicleType.Car, "NOPE", "Black", 4, 1, FuelType: FuelType.Gasoline);
        GarageDto dto = new(GarageMapper.CurrentVersion, 5, [car]);

        Assert.ThrowsAny<ArgumentException>(() => GarageMapper.ToGarage(dto));
    }

    [Fact]
    public void ToGarage_MoreVehiclesThanCapacity_Throws()
    {
        VehicleDto car1 = new(VehicleType.Car, "ABC123", "Black", 4, 5, FuelType: FuelType.Gasoline);
        VehicleDto car2 = new(VehicleType.Car, "DEF456", "Black", 4, 5, FuelType: FuelType.Gasoline);
        GarageDto dto = new(GarageMapper.CurrentVersion, 1, [car1, car2]);

        Assert.Throws<InvalidDataException>(() => GarageMapper.ToGarage(dto));
    }

    [Fact]
    public void ToGarage_DuplicateRegistrationNumbers_Throws()
    {
        VehicleDto car1 = new(VehicleType.Car, "ABC123", "Black", 4, 5, FuelType: FuelType.Gasoline);
        VehicleDto car2 = new(VehicleType.Car, "abc123", "Black", 4, 5, FuelType: FuelType.Gasoline);
        GarageDto dto = new(GarageMapper.CurrentVersion, 5, [car1, car2]);

        Assert.Throws<InvalidDataException>(() => GarageMapper.ToGarage(dto));
    }
}
