using System.Text.Json.Serialization;
using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Serializable form of a vehicle. Each vehicle type has its own derived DTO carrying only its type-specific fields.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CarDto), "Car")]
[JsonDerivedType(typeof(BusDto), "Bus")]
[JsonDerivedType(typeof(AirplaneDto), "Airplane")]
[JsonDerivedType(typeof(TruckDto), "Truck")]
[JsonDerivedType(typeof(MotorcycleDto), "Motorcycle")]
[JsonDerivedType(typeof(BoatDto), "Boat")]
public abstract record VehicleDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize);

public sealed record CarDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    FuelType FuelType) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);

public sealed record BusDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    int NumberOfSeats) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);

public sealed record AirplaneDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    int NumberOfEngines) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);

public sealed record TruckDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    int LoadCapacity) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);

public sealed record MotorcycleDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    int CylinderVolume) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);

public sealed record BoatDto(
    string RegistrationNumber,
    string Color,
    int NumberOfWheels,
    int ParkingSize,
    double Length) : VehicleDto(RegistrationNumber, Color, NumberOfWheels, ParkingSize);
