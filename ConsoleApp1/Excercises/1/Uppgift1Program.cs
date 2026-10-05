using System;

namespace Uppgift1
{
    /// <summary>
    /// Entry point. Handles the console menu and user input.
    /// </summary>
    public class Program
    {
        private static EmployeeRegister register;

        public static void Main(string[] args)
        {
            register = new EmployeeRegister();

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                // End of input (e.g. redirected input ran out): stop.
                if (choice == null)
                {
                    return;
                }

                switch (choice.Trim())
                {
                    case "1":
                        ReadEmployee();
                        break;
                    case "2":
                        register.PrintRegister();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Add employee");
            Console.WriteLine("2. Print register");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
        }

        private static void ReadEmployee()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Salary: ");
            decimal? salary = ReadSalary();

            if (salary == null)
            {
                Console.WriteLine("Invalid salary. Employee was not added.");
                return;
            }

            try
            {
                register.AddEmployee(new Employee(name, salary.Value));
                Console.WriteLine("Employee added.");
            }
            catch (ArgumentException ex)
            {
                // Employee rejects empty names and negative salaries.
                Console.WriteLine("Could not add employee: " + ex.Message);
            }
        }

        // Returns null if the input is not a number.
        private static decimal? ReadSalary()
        {
            string input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal salary))
            {
                return salary;
            }

            return null;
        }
    }
}