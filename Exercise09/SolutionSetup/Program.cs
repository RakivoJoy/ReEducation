using SolutionSetup.Persistence;
using SolutionSetup.UI;
using System;

namespace SolutionSetup;

/// <summary>
/// Entry point for the Garage 1.0 console application.
/// </summary>
internal class Program
{
    /// <summary>
    /// Starts the application by creating the console user interface and running its menu loop
    /// until the user chooses to exit.
    /// </summary>
    static void Main(string[] args)
    {
        IGarageStorage storage = new JsonGarageStorage(JsonGarageStorage.DefaultFilePath);
        IHandler handler = new GarageHandler(storage, new SampleDataProvider());
        IUI ui = new ConsoleUI(Console.In, Console.Out, handler);
        ui.Run();
    }
}
