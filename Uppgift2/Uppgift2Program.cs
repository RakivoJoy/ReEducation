using System;

namespace Uppgift2
{
    // Main program shell for the exercise "Flow with loops and string manipulation".

    public static class Uppgift2Program
    {
        // Entry point used by the final application.

        public static void Main(string[] args)
        {

            Console.Out.WriteLine("Welcome to the ticket price calculator!");
            // TODO: Implement the interactive console menu here.
            // Pseudocode / steps to follow when implementing:
            // 1. Set a boolean flag `running = true` and while(running) { ... }
            // 2. Print menu text showing options 0 (exit), 1 (youth or pensioner price), 2 (group price)
            // 3. Read user input as string, parse selection, and use a switch on the input
            // 4. For case "1" call into the ticket-price routine for a single person
            // 5. For case "2" call into the group pricing routine
            // 6. For case "0" set running = false to exit
        }
    }
}
