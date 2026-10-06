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

                // End of input (e.g. redirected input ran out): stop.
                if (choice == null)
                {
                    return;
                }

                //TODO: Use enum for menu choices instead of magic strings.
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

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Add employee");
            Console.WriteLine("2. Print register");
            Console.WriteLine("3. Save to file");
            Console.WriteLine("4. Load from file");
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

        static void SaveRegister()
        {
            Console.Write("File path: ");
            string path = Console.ReadLine();

            try
            {
                register.SaveToFile(path);
                Console.WriteLine("Saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not save: " + ex.Message);
            }
        }

        static void LoadRegister()
        {
            Console.Write("File path: ");
            string path = Console.ReadLine();

            try
            {
                register.LoadFromFile(path);
                Console.WriteLine("Loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load: " + ex.Message);
            }
        }
    }
}
