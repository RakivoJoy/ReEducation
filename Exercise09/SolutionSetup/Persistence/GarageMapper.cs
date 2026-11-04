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
        Car car => new CarDto(car.RegistrationNumber, car.Color, car.NumberOfWheels, car.ParkingSize, car.FuelType),
        Bus bus => new BusDto(bus.RegistrationNumber, bus.Color, bus.NumberOfWheels, bus.ParkingSize, bus.NumberOfSeats),
        Airplane airplane => new AirplaneDto(airplane.RegistrationNumber, airplane.Color, airplane.NumberOfWheels, airplane.ParkingSize, airplane.NumberOfEngines),
        Truck truck => new TruckDto(truck.RegistrationNumber, truck.Color, truck.NumberOfWheels, truck.ParkingSize, truck.LoadCapacity),
        Motorcycle motorcycle => new MotorcycleDto(motorcycle.RegistrationNumber, motorcycle.Color, motorcycle.NumberOfWheels, motorcycle.ParkingSize, motorcycle.CylinderVolume),
        Boat boat => new BoatDto(boat.RegistrationNumber, boat.Color, boat.NumberOfWheels, boat.ParkingSize, boat.Length),
        _ => throw new NotSupportedException($"Vehicle type {vehicle.GetType().Name} cannot be saved.")
    };

    private static IVehicle ToVehicle(VehicleDto dto) => dto switch
    {
        CarDto car => new Car(car.RegistrationNumber, car.Color, car.NumberOfWheels, car.FuelType, car.ParkingSize),
        BusDto bus => new Bus(bus.RegistrationNumber, bus.Color, bus.NumberOfWheels, bus.NumberOfSeats, bus.ParkingSize),
        AirplaneDto airplane => new Airplane(airplane.RegistrationNumber, airplane.Color, airplane.NumberOfWheels, airplane.NumberOfEngines, airplane.ParkingSize),
        TruckDto truck => new Truck(truck.RegistrationNumber, truck.Color, truck.NumberOfWheels, truck.LoadCapacity, truck.ParkingSize),
        MotorcycleDto motorcycle => new Motorcycle(motorcycle.RegistrationNumber, motorcycle.Color, motorcycle.NumberOfWheels, motorcycle.CylinderVolume, motorcycle.ParkingSize),
        BoatDto boat => new Boat(boat.RegistrationNumber, boat.Color, boat.NumberOfWheels, boat.Length, boat.ParkingSize),
        _ => throw new ArgumentException($"Unknown vehicle DTO {dto.GetType().Name}.", nameof(dto))
    };
}
