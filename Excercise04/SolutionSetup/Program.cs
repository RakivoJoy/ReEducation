using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetup
{
    internal class Program
    {
        public static void Main()
        {
            var linesInput = Console.ReadLine();
            if (!int.TryParse(linesInput, out var lines))
            {
                // Invalid input for the number of lines; exit the program
                return;
            }

            var persons = new List<Person>(lines);

            for (int i = 0; i < lines; i++)
            {
                var input = Console.ReadLine();
                if (input is null)
                {
                    // No more input available; stop reading further lines
                    break;
                }

                var cmdArgs = input.Split();
                if (cmdArgs.Length < 3)
                {
                    // Invalid input format; skip this line
                    continue;
                }

                if (!int.TryParse(cmdArgs[2], out var age))
                {
                    // Invalid age value; skip this line
                    continue;
                }

                var person = new Person(cmdArgs[0], cmdArgs[1], age);
                persons.Add(person);
            }

            persons.OrderBy(p => p.FirstName)
                   .ThenBy(p => p.Age)
                   .ToList()
                   .ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
