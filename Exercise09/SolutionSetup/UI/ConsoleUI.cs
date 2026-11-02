using SolutionSetup.Vehicles;
using System;
using System.IO;

namespace SolutionSetup.UI;

/// <summary>
/// Text-based user interface for the garage. Reads menu choices and vehicle details from the input
/// and writes results to the output. Both streams are injectable so the UI can be tested.
/// </summary>
public partial class ConsoleUI
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    // The handler owns the garage; the UI only reads input, calls the handler and prints results.
    private readonly IHandler _handler;
    private readonly MenuItem[] _menu;
    private readonly VehicleTypeInfo[] _vehicleTypes;

    internal VehicleTypeInfo[] VehicleTypes => _vehicleTypes;

    private sealed record MenuItem(string Key, string Text, Action? Action, bool PauseAfter, bool RequiresGarage = false);

    /// <summary>
    /// Creates a console UI that reads from <see cref="Console.In"/> and writes to <see cref="Console.Out"/>.
    /// </summary>
    public ConsoleUI()
        : this(Console.In, Console.Out, new GarageHandler())
    {
    }

    /// <summary>
    /// Creates a console UI that uses the given input and output streams and a new <see cref="GarageHandler"/>.
    /// </summary>
    /// <param name="input">Source of user input.</param>
    /// <param name="output">Destination for menu text and messages.</param>
    internal ConsoleUI(TextReader input, TextWriter output)
        : this(input, output, new GarageHandler())
    {
    }

    /// <summary>
    /// Creates a console UI that uses the given input, output and handler.
    /// </summary>
    /// <param name="input">Source of user input.</param>
    /// <param name="output">Destination for menu text and messages.</param>
    /// <param name="handler">Handler that owns the garage and performs the menu actions.</param>
    internal ConsoleUI(TextReader input, TextWriter output, IHandler handler)
    {
        _input = input;
        _output = output;
        _handler = handler;
        _menu =
        [
            new MenuItem("1", "Create garage", CreateGarage, PauseAfter: true),
            new MenuItem("2", "Populate garage with sample vehicles", PopulateGarage, PauseAfter: true, RequiresGarage: true),
            new MenuItem("3", "Park vehicle", ParkVehicle, PauseAfter: true, RequiresGarage: true),
            new MenuItem("4", "Remove vehicle", RemoveVehicle, PauseAfter: true, RequiresGarage: true),
            new MenuItem("5", "List all vehicles", ListVehicles, PauseAfter: true, RequiresGarage: true),
            new MenuItem("6", "List vehicle count by type", ListVehicleCounts, PauseAfter: true, RequiresGarage: true),
            new MenuItem("7", "Find vehicle by registration number", FindVehicle, PauseAfter: true, RequiresGarage: true),
            new MenuItem("8", "Search vehicles by properties", SearchVehicles, PauseAfter: true, RequiresGarage: true),
            new MenuItem("0", "Exit", Action: null, PauseAfter: false),
        ];
        _vehicleTypes = BuildVehicleTypes();
    }

    /// <summary>
    /// Runs the main menu loop. Prints the menu, reads a choice, and dispatches it to the matching
    /// handler until the user selects 0. Unexpected exceptions are caught so the loop keeps running.
    /// </summary>
    public void Run()
    {
        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            string? choice = _input.ReadLine();
            if (choice is null)
            {
                break;
            }

            try
            {
                exit = HandleChoice(choice);
            }
            catch (IOException)
            {
                // The input or output stream failed (for example a closed console or redirected pipe).
                // Writing an error message would likely fail too, so stop the menu loop instead.
                exit = true;
            }
            catch (ObjectDisposedException)
            {
                // The input or output stream was disposed while the menu was running.
                // It can no longer be used, so stop the menu loop.
                exit = true;
            }
            catch (Exception ex)
            {
                // Last-resort safety net so an unanticipated error does not crash the application.
                _output.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Runs the menu item matching the choice. Returns true when the user selected Exit.
    /// </summary>
    /// <param name="choice">The menu key entered by the user.</param>
    private bool HandleChoice(string choice)
    {
        MenuItem? item = Array.Find(_menu, menuItem => menuItem.Key == choice);
        if (item is null)
        {
            _output.WriteLine(ConsoleUIText.InvalidChoice);
            return false;
        }

        if (item.Action is null)
        {
            return true;
        }

        if (!item.RequiresGarage || EnsureGarage())
        {
            item.Action();
        }

        if (item.PauseAfter)
        {
            PauseForEnter();
        }

        return false;
    }

    /// <summary>
    /// Prints a blank line and waits for Enter so the result of the last action stays visible before the menu is shown again.
    /// </summary>
    private void PauseForEnter()
    {
        _output.WriteLine();
        _output.WriteLine(ConsoleUIText.PressEnterToContinue);
        _input.ReadLine();
    }

    /// <summary>
    /// Prints the numbered main menu from the menu table.
    /// </summary>
    private void PrintMenu()
    {
        _output.WriteLine(ConsoleUIText.MenuTitle);
        foreach (MenuItem item in _menu)
        {
            _output.WriteLine($"{item.Key}. {item.Text}");
        }
    }
}
