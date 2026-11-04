using System;
using System.Collections.Generic;
using System.IO;
using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Converts between the garage domain objects and their serializable DTOs. Performs no file IO.
/// </summary>
public static class GarageMapper
{
    /// <summary>
    /// The file format version written by ToDto.
    /// </summary>
    public const int CurrentVersion = 1;

    /// <summary>
    /// Creates a DTO holding the capacity and every parked vehicle.
    /// </summary>
    public static GarageDto ToDto(IGarage<IVehicle> garage)
    {
        ArgumentNullException.ThrowIfNull(garage);

        List<VehicleDto> vehicles = [];
        foreach (IVehicle vehicle in garage)
        {
            vehicles.Add(ToDto(vehicle));
        }

        return new GarageDto(CurrentVersion, garage.Capacity, vehicles);
    }

    /// <summary>
    /// Rebuilds a garage from a DTO. Vehicles are re-validated by their constructors and added through
    /// <see cref="Garage{T}.AddVehicle"/>, so capacity and duplicate rules apply.
    /// </summary>
    /// <exception cref="InvalidDataException">Thrown when the version is unsupported or a vehicle cannot be added.</exception>
    /// <exception cref="ArgumentException">Thrown when a vehicle DTO is missing a required field or has invalid data.</exception>
    public static IGarage<IVehicle> ToGarage(GarageDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.Version != CurrentVersion)
        {
            throw new InvalidDataException($"Unsupported garage file version {dto.Version}.");
        }

        Garage<IVehicle> garage = new(dto.Capacity);
        foreach (VehicleDto vehicleDto in dto.Vehicles)
        {
            IVehicle vehicle = ToVehicle(vehicleDto);
            ParkResult result = garage.AddVehicle(vehicle);
            if (!result.Parked)
            {
                string reason = result.Reason == ParkFailureReason.Duplicate
                    ? $"A vehicle with registration number {vehicle.RegistrationNumber} is already parked."
                    : "The garage is full.";
                throw new InvalidDataException(reason);
            }
        }

        return garage;
    }

    private static VehicleDto ToDto(IVehicle vehicle) => vehicle switch
    {
        Car car => new VehicleDto(car.Type, car.RegistrationNumber, car.Color, car.NumberOfWheels, car.ParkingSize, FuelType: car.FuelType),
        Bus bus => new VehicleDto(bus.Type, bus.RegistrationNumber, bus.Color, bus.NumberOfWheels, bus.ParkingSize, NumberOfSeats: bus.NumberOfSeats),
        Airplane airplane => new VehicleDto(airplane.Type, airplane.RegistrationNumber, airplane.Color, airplane.NumberOfWheels, airplane.ParkingSize, NumberOfEngines: airplane.NumberOfEngines),
        Truck truck => new VehicleDto(truck.Type, truck.RegistrationNumber, truck.Color, truck.NumberOfWheels, truck.ParkingSize, LoadCapacity: truck.LoadCapacity),
        Motorcycle motorcycle => new VehicleDto(motorcycle.Type, motorcycle.RegistrationNumber, motorcycle.Color, motorcycle.NumberOfWheels, motorcycle.ParkingSize, CylinderVolume: motorcycle.CylinderVolume),
        Boat boat => new VehicleDto(boat.Type, boat.RegistrationNumber, boat.Color, boat.NumberOfWheels, boat.ParkingSize, Length: boat.Length),
        _ => throw new NotSupportedException($"Vehicle type {vehicle.GetType().Name} cannot be saved.")
    };

    private static IVehicle ToVehicle(VehicleDto dto) => dto.Type switch
    {
        VehicleType.Car => new Car(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.FuelType, nameof(dto.FuelType)), dto.ParkingSize),
        VehicleType.Bus => new Bus(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.NumberOfSeats, nameof(dto.NumberOfSeats)), dto.ParkingSize),
        VehicleType.Airplane => new Airplane(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.NumberOfEngines, nameof(dto.NumberOfEngines)), dto.ParkingSize),
        VehicleType.Truck => new Truck(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.LoadCapacity, nameof(dto.LoadCapacity)), dto.ParkingSize),
        VehicleType.Motorcycle => new Motorcycle(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.CylinderVolume, nameof(dto.CylinderVolume)), dto.ParkingSize),
        VehicleType.Boat => new Boat(dto.RegistrationNumber, dto.Color, dto.NumberOfWheels, Required(dto.Length, nameof(dto.Length)), dto.ParkingSize),
        _ => throw new ArgumentException($"Unknown vehicle type {dto.Type}.", nameof(dto))
    };

    private static T Required<T>(T? value, string fieldName) where T : struct =>
        value ?? throw new ArgumentException($"Missing required field {fieldName}.", fieldName);
}
