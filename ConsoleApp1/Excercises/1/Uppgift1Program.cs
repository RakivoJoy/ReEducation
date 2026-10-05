using System;

namespace Uppgift1
{
    public class Program
    {
        static EmployeeRegister register;

        public static void Main(string[] args)
        {
            register = new EmployeeRegister();

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                if (choice == null) return; // no more input

                choice = choice.Trim();

                if (choice == "1") ReadEmployee();
                else if (choice == "2") register.PrintRegister();
                else if (choice == "0") return;
                else Console.WriteLine("Invalid choice.");
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Add employee");
            Console.WriteLine("2. Print register");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
        }

        static void ReadEmployee()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Salary: ");
            decimal salary;
            if (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Invalid salary. Employee was not added.");
                return;
            }

            try
            {
                register.AddEmployee(new Employee(name, salary));
                Console.WriteLine("Employee added.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Could not add employee: " + ex.Message);
            }
        }
    }
}