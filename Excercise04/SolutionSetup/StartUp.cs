using System;
using System.Collections.Generic;

namespace SolutionSetup
{
    public class StartUp
    {
        public static void Main()
        {
            var linesStr = Console.ReadLine();
            if (!int.TryParse(linesStr, out var lines)) return;

            var persons = new List<Person>();
            for (int i = 0; i < lines; i++)
            {
                var input = Console.ReadLine();
                if (input == null) continue;
                var cmdArgs = input.Split();
                if (cmdArgs.Length < 4) continue;

                if (!int.TryParse(cmdArgs[2], out var age)) continue;
                if (!decimal.TryParse(cmdArgs[3], out var salary)) continue;

                var person = new Person(cmdArgs[0], cmdArgs[1], age, salary);
                persons.Add(person);
            }

            var bonusStr = Console.ReadLine();
            if (!decimal.TryParse(bonusStr, out var bonus)) bonus = 0m;

            persons.ForEach(p => p.IncreaseSalary(bonus));
            persons.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
