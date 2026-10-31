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
        if (!EnsureGarage())
        {
            return;
        }

        VehicleTypeInfo[] types = VehicleTypes;
        _output.WriteLine("Select vehicle type:");
        for (int i = 0; i < types.Length; i++)
        {
            _output.WriteLine($"{i + 1}. {types[i].Type}");
        }

        try
        {
            VehicleTypeInfo typeInfo = ReadValidated<VehicleTypeInfo>(
                $"Enter vehicle type number (1-{types.Length}):",
                line => $"Invalid input: '{line}' is not a valid vehicle type option. Please choose 1-{types.Length}.",
                (string text, out VehicleTypeInfo value) =>
                {
                    bool valid = int.TryParse(text, out int number) && number >= 1 && number <= types.Length;
                    value = valid ? types[number - 1] : types[0];
                    return valid;
                });

            string registrationNumber = ReadValidated<string>(
                "Enter registration number:",
                line => $"Invalid registration number '{line}'. Use three letters (excluding I, Q, V) followed by three alphanumerics.",
                (string text, out string value) => Vehicle.TryNormalizeRegistrationNumber(text, out value));

            _output.WriteLine($"Most common colors: {string.Join(", ", VehicleColors.MostCommon)}");
            string color = ReadValidated<string>(
                "Enter color:",
                line => $"Invalid color '{line}'. Choose one of the allowed colors.",
                (string text, out string value) => VehicleColors.TryNormalize(text, out value));

            int numberOfWheels = ReadIntPrompt("Enter number of wheels:", "Invalid number of wheels. Please enter a whole number.");

            Vehicle vehicle = typeInfo.Create(registrationNumber, color, numberOfWheels);

            OperationResult result = _handler.ParkVehicle(vehicle);
            _output.WriteLine(result.Message);
        }
        catch (EndOfStreamException)
        {
            _output.WriteLine(ConsoleUIText.InputEndedBeforeVehicleParked);
        }
    }

    /// <summary>
    /// Describes one selectable vehicle type: the enum value it represents and a factory that builds it.
    /// </summary>
    /// <param name="Type">The vehicle type this entry creates.</param>
    /// <param name="Create">
    /// Builds the vehicle from the registration number, color and number of wheels. It also prompts for any
    /// type-specific detail, such as fuel type or seat count.
    /// </param>
    internal sealed record VehicleTypeInfo(VehicleType Type, Func<string, string, int, Vehicle> Create);

    private VehicleTypeInfo[]? _vehicleTypes;

    internal VehicleTypeInfo[] VehicleTypes => _vehicleTypes ??=
    [
        new(VehicleType.Motorcycle, (reg, color, wheels) => new Motorcycle(reg, color, wheels, ReadIntPrompt("Enter cylinder volume (cc):"))),
        new(VehicleType.Airplane, (reg, color, wheels) => new Airplane(reg, color, wheels, ReadIntPrompt("Enter number of engines:"))),
        new(VehicleType.Car, (reg, color, wheels) => new Car(reg, color, wheels, ReadFuelTypePrompt())),
        new(VehicleType.Bus, (reg, color, wheels) => new Bus(reg, color, wheels, ReadIntPrompt("Enter number of seats:"))),
        new(VehicleType.Boat, (reg, color, wheels) => new Boat(reg, color, wheels, ReadDoublePrompt("Enter length (m):"))),
        new(VehicleType.Truck, (reg, color, wheels) => new Truck(reg, color, wheels, ReadIntPrompt("Enter load capacity (kg):")))
    ];

    /// <summary>
    /// Menu option 4. Asks for a registration number and removes the matching vehicle.
    /// </summary>
    internal void RemoveVehicle()
    {
        if (!EnsureGarage())
        {
            return;
        }

        _output.WriteLine("Enter registration number to remove:");
        string registrationNumber = ReadLineOrEmpty();

        OperationResult result = _handler.RemoveVehicle(registrationNumber);
        _output.WriteLine(result.Message);
    }

    /// <summary>
    /// Menu option 5. Prints every parked vehicle, or a message if the garage is empty.
    /// </summary>
    internal void ListVehicles()
    {
        if (!EnsureGarage())
        {
            return;
        }

        IReadOnlyList<IVehicle> vehicles = _handler.ListVehicles();
        if (vehicles.Count == 0)
        {
            _output.WriteLine(ConsoleUIText.NoVehiclesParked);
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
        if (!EnsureGarage())
        {
            return;
        }

        _output.WriteLine("Enter registration number to find:");
        string registrationNumber = ReadLineOrEmpty();

        IVehicle? vehicle = _handler.FindVehicle(registrationNumber);
        if (vehicle is null)
        {
            _output.WriteLine($"No vehicle with registration number {registrationNumber} was found.");
            return;
        }

        _output.WriteLine(vehicle.ToString());
    }
}
