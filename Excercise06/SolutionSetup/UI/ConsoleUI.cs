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
    // The garage is null until the user creates one (menu option 1).
    private Garage? _garage;

    /// <summary>
    /// Creates a console UI that reads from <see cref="Console.In"/> and writes to <see cref="Console.Out"/>.
    /// </summary>
    public ConsoleUI()
        : this(Console.In, Console.Out)
    {
    }

    /// <summary>
    /// Creates a console UI that uses the given input and output streams.
    /// </summary>
    /// <param name="input">Source of user input.</param>
    /// <param name="output">Destination for menu text and messages.</param>
    internal ConsoleUI(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
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
            string choice = ReadLineOrEmpty();

            try
            {
                // Each menu option maps to one handler method in the partial class files.
                switch (choice)
                {
                    case "1":
                        CreateGarage();
                        break;
                    case "2":
                        PopulateGarage();
                        break;
                    case "3":
                        ParkVehicle();
                        break;
                    case "4":
                        RemoveVehicle();
                        break;
                    case "5":
                        ListVehicles();
                        break;
                    case "6":
                        ListVehicleCounts();
                        break;
                    case "7":
                        FindVehicle();
                        break;
                    case "8":
                        SearchVehicles();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        _output.WriteLine("Invalid choice. Please select a valid menu option.");
                        break;
                }
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
    /// Prints the numbered main menu (options 1 to 8 and 0 for exit).
    /// </summary>
    private void PrintMenu()
    {
        _output.WriteLine("Welcome to your Garage 1.0");
        _output.WriteLine("1. Create garage");
        _output.WriteLine("2. Populate garage with sample vehicles");
        _output.WriteLine("3. Park vehicle");
        _output.WriteLine("4. Remove vehicle");
        _output.WriteLine("5. List all vehicles");
        _output.WriteLine("6. List vehicle count by type");
        _output.WriteLine("7. Find vehicle by registration number");
        _output.WriteLine("8. Search vehicles by properties");
        _output.WriteLine("0. Exit");
    }
}
