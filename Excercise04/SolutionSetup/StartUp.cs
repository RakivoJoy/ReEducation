using System;
using System.Collections.Generic;
using System.Globalization;

namespace SolutionSetup
{
    /// <summary>
    /// Main application entry point for reading and processing person salary data.
    /// Handles console I/O with robust error handling for invalid input data.
    /// </summary>
    public class StartUp
    {
        public static void Main()
        {
            try
            {
                int personCount = ReadPersonCount();
                if (personCount <= 0)
                    return;

                var persons = ReadPersons(personCount);
                decimal bonus = ReadBonus();

                ApplyBonusToPersons(persons, bonus);
                OutputPersons(persons);
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions at the top level
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>Reads and validates the number of persons to process.</summary>
        private static int ReadPersonCount()
        {
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var count))
                return 0;

            return count;
        }

        /// <summary>Reads person records from console input and creates Person objects.</summary>
        /// <remarks>
        /// Skips lines with invalid data (null, insufficient fields, invalid age/salary).
        /// Catches exceptions from Person constructor for invalid name or salary bounds.
        /// </remarks>
        private static List<Person> ReadPersons(int count)
        {
            var persons = new List<Person>();

            for (int i = 0; i < count; i++)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (input == null)
                        continue;

                    var fields = input.Split();
                    if (fields.Length < 4)
                        continue;

                    // Validate numeric fields
                    if (!int.TryParse(fields[2], out var age))
                        continue;
                    if (!decimal.TryParse(fields[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var salary))
                        continue;

                    // Create person (may throw ArgumentException for validation failures)
                    var person = new Person(fields[0], fields[1], age, salary);
                    persons.Add(person);
                }
                catch (ArgumentException)
                {
                    // Skip invalid person data - continue to next record
                    continue;
                }
            }

            return persons;
        }

        /// <summary>Reads and parses the bonus percentage, defaulting to 0 if invalid.</summary>
        private static decimal ReadBonus()
        {
            var input = Console.ReadLine();
            if (!decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var bonus))
                return 0m;

            return bonus;
        }

        /// <summary>Applies the bonus percentage to all persons' salaries with error handling.</summary>
        /// <remarks>
        /// If the bonus is invalid (negative), outputs a warning but continues to output persons.
        /// This ensures the program doesn't crash due to bonus application failures.
        /// </remarks>
        private static void ApplyBonusToPersons(List<Person> persons, decimal bonus)
        {
            try
            {
                persons.ForEach(p => p.IncreaseSalary(bonus));
            }
            catch (ArgumentException ex)
            {
                // If bonus is invalid (e.g., negative), output warning but continue
                Console.WriteLine($"Warning: Could not apply bonus - {ex.Message}");
            }
        }

        /// <summary>Outputs all persons to the console.</summary>
        private static void OutputPersons(List<Person> persons)
        {
            persons.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
