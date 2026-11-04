using SolutionSetup.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolutionSetup.UI;

/// <summary>
/// Menu handlers that park, remove, list and find individual vehicles.
/// </summary>
public partial class ConsoleUI
{
    /// <summary>
    /// Menu option 3. Asks for a vehicle type and its details, then parks the vehicle.
    /// Invalid input is reported to the user and nothing is parked.
    /// </summary>
    internal void ParkVehicle()
    {
        try
        {
            VehicleTypeInfo typeInfo = ReadVehicleType();
            string registrationNumber = ReadRegistrationNumber();
            string color = ReadColor();
            int numberOfWheels = ReadIntPrompt(ConsoleUIText.EnterNumberOfWheels, ConsoleUIText.InvalidNumberOfWheels);
            int parkingSize = ReadParkingSize(typeInfo.Type);

            VehicleRequest request = typeInfo.Create(registrationNumber, color, numberOfWheels, parkingSize);

            OperationResult result = _handler.ParkVehicle(request);
            _output.WriteLine(result.Message);
        }
        catch (EndOfStreamException)
        {
            _output.WriteLine(ConsoleUIText.InputEndedBeforeVehicleParked);
        }
    }

    private VehicleTypeInfo ReadVehicleType()
    {
        VehicleTypeInfo[] types = _vehicleTypes;
        _output.WriteLine(ConsoleUIText.SelectVehicleType);
        for (int i = 0; i < types.Length; i++)
        {
            _output.WriteLine($"{i + 1}. {types[i].Type}");
        }

        return ReadValidated<VehicleTypeInfo>(
            $"Enter vehicle type number (1-{types.Length}):",
            line => $"Invalid input: '{line}' is not a valid vehicle type option. Please choose 1-{types.Length}.",
            (string text, out VehicleTypeInfo value) => TryParseVehicleType(text, types, out value));
    }

    private static bool TryParseVehicleType(string text, VehicleTypeInfo[] types, out VehicleTypeInfo value)
    {
        bool valid = int.TryParse(text, out int number) && number >= 1 && number <= types.Length;
        value = valid ? types[number - 1] : types[0];
        return valid;
    }

    private string ReadRegistrationNumber()
    {
        return ReadValidated<string>(
            ConsoleUIText.EnterRegistrationNumber,
            line => $"Invalid registration number '{line}'. Use {Vehicle.RegistrationNumberRule}.",
            (string text, out string value) => Vehicle.TryNormalizeRegistrationNumber(text, out value));
    }

    private string ReadColor()
    {
        _output.WriteLine($"Most common colors: {string.Join(", ", VehicleColors.MostCommon)}");
        return ReadValidated<string>(
            ConsoleUIText.EnterColor,
            line => $"Invalid color '{line}'. Choose one of the allowed colors.",
            (string text, out string value) => VehicleColors.TryNormalize(text, out value));
    }

    private int ReadParkingSize(VehicleType type)
    {
        (int minSize, int maxSize) = ParkingSizeLimits.For(type);
        return ReadValidated<int>(
            $"Enter parking size ({minSize}-{maxSize}):",
            line => $"Invalid parking size '{line}'. Enter a whole number between {minSize} and {maxSize}.",
            (string text, out int value) => int.TryParse(text, out value) && value >= minSize && value <= maxSize);
    }

    /// <summary>
    /// Describes one selectable vehicle type: the enum value it represents and a factory that builds it.
    /// </summary>
    /// <param name="Type">The vehicle type this entry creates.</param>
    /// <param name="Create">
    /// Builds the request from the registration number, color, number of wheels and parking size. It also prompts for any
    /// type-specific detail, such as fuel type or seat count.
    /// </param>
    internal sealed record VehicleTypeInfo(VehicleType Type, Func<string, string, int, int, VehicleRequest> Create);

    private VehicleTypeInfo[] BuildVehicleTypes() =>
    [
        new(VehicleType.Motorcycle, (reg, color, wheels, size) => new(VehicleType.Motorcycle, reg, color, wheels, size, Details(VehicleFactory.CylinderVolumeKey, ReadIntPrompt("Enter cylinder volume (cc):")))),
        new(VehicleType.Airplane, (reg, color, wheels, size) => new(VehicleType.Airplane, reg, color, wheels, size, Details(VehicleFactory.NumberOfEnginesKey, ReadIntPrompt("Enter number of engines:")))),
        new(VehicleType.Car, (reg, color, wheels, size) => new(VehicleType.Car, reg, color, wheels, size, Details(VehicleFactory.FuelTypeKey, ReadFuelTypePrompt()))),
        new(VehicleType.Bus, (reg, color, wheels, size) => new(VehicleType.Bus, reg, color, wheels, size, Details(VehicleFactory.NumberOfSeatsKey, ReadIntPrompt("Enter number of seats:")))),
        new(VehicleType.Boat, (reg, color, wheels, size) => new(VehicleType.Boat, reg, color, wheels, size, Details(VehicleFactory.LengthKey, ReadDoublePrompt("Enter length (m):")))),
        new(VehicleType.Truck, (reg, color, wheels, size) => new(VehicleType.Truck, reg, color, wheels, size, Details(VehicleFactory.LoadCapacityKey, ReadIntPrompt("Enter load capacity (kg):"))))
    ];

    private static IReadOnlyDictionary<string, object> Details(string key, object value) => new Dictionary<string, object> { [key] = value };

    /// <summary>
    /// Menu option 4. Asks for a registration number and removes the matching vehicle.
    /// </summary>
    internal void RemoveVehicle()
    {
        _output.WriteLine(ConsoleUIText.EnterRegistrationNumberToRemove);
        string registrationNumber = ReadLineOrEmpty();

        OperationResult result = _handler.RemoveVehicle(registrationNumber);
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 5. Prints every parked vehicle, or a message if the garage is empty.
    /// </summary>
    internal void ListVehicles()
    {
        PrintVehicles(_handler.ListVehicles(), ConsoleUIText.NoVehiclesParked);
    }

    private void PrintVehicles(IReadOnlyList<IVehicle> vehicles, string emptyMessage)
    {
        if (vehicles.Count == 0)
        {
            _output.WriteLine(emptyMessage);
            return;
        }

        foreach (IVehicle vehicle in vehicles)
        {
            _output.WriteLine(vehicle.ToString());
        }
    }

    /// <summary>
    /// Menu option 7. Asks for a registration number and prints the matching vehicle, if any.
    /// </summary>
    internal void FindVehicle()
    {
        _output.WriteLine(ConsoleUIText.EnterRegistrationNumberToFind);
        string registrationNumber = ReadLineOrEmpty();

        IVehicle? vehicle = _handler.FindVehicle(registrationNumber);
        if (vehicle is null)
        {
            _output.WriteLine(ConsoleUIText.VehicleNotFound(registrationNumber));
            return;
        }

        _output.WriteLine(vehicle.ToString());
    }
}
