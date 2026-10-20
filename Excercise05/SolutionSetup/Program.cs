using SolutionSetup.UI;

namespace SolutionSetup
{
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
            new ConsoleUI().Run();
        }
    }
}
