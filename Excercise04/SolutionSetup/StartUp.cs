using System;
using System.Collections.Generic;
using System.Globalization;

namespace SolutionSetup
{
    public class StartUp
    {
        public static void Main()
        {
            try
            {
                var linesStr = Console.ReadLine();
                if (!int.TryParse(linesStr, out var lines))
                {
                    return;
                }

                var persons = new List<Person>();

                // Try-catch block to handle person creation failures
                for (int i = 0; i < lines; i++)
                {
                    try
                    {
                        var input = Console.ReadLine();
                        if (input == null) continue;

                        var cmdArgs = input.Split();
                        if (cmdArgs.Length < 4) continue;

                        // TryParse still used for numeric validation (not throwing)
                        if (!int.TryParse(cmdArgs[2], out var age)) continue;
                        if (!decimal.TryParse(cmdArgs[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var salary)) continue;

                        // Person constructor can throw ArgumentException - catch it for invalid data
                        var person = new Person(cmdArgs[0], cmdArgs[1], age, salary);
                        persons.Add(person);
                    }
                    catch (ArgumentException ex)
                    {
                        // Skip invalid person data (name too short, age invalid, salary too low)
                        // Continue to next person instead of crashing
                        continue;
                    }
                }

                // Read and parse bonus with try-catch for IncreaseSalary exceptions
                var bonusStr = Console.ReadLine();
                decimal bonus = 0m;
                if (!decimal.TryParse(bonusStr, NumberStyles.Any, CultureInfo.InvariantCulture, out bonus))
                {
                    bonus = 0m;
                }

                // Try-catch block to handle bonus application failures
                try
                {
                    persons.ForEach(p => p.IncreaseSalary(bonus));
                }
                catch (ArgumentException ex)
                {
                    // If bonus is somehow invalid (e.g., negative), skip bonus application
                    // but still output the persons
                    Console.WriteLine($"Warning: Could not apply bonus - {ex.Message}");
                }

                // Output all valid persons
                persons.ForEach(p => Console.WriteLine(p.ToString()));
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions at the top level
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
