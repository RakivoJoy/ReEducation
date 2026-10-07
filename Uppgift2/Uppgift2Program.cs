using System;
using System.Collections.Generic;

namespace Uppgift2
{
    // Main program shell for the exercise "Flow with loops and string manipulation".

    public static class Uppgift2Program
    {
        // Entry point used by the final application.

        public static void Main(string[] args)
        {
            var service = new Uppgift2TicketService();
            bool running = true;

            Console.Out.WriteLine("Welcome to the ticket price calculator!");

            while (running)
            {
                Console.Out.WriteLine("\n--- Menu ---");
                Console.Out.WriteLine("0. Exit");
                Console.Out.WriteLine("1. Calculate single person price");
                Console.Out.WriteLine("2. Calculate group price");
                Console.Out.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleSinglePerson(service);
                        break;
                    case "2":
                        HandleGroupPrice(service);
                        break;
                    case "0":
                        running = false;
                        Console.Out.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.Out.WriteLine("Invalid choice. Please enter 0, 1, or 2.");
                        break;
                }
            }
        }

        private static void HandleSinglePerson(Uppgift2TicketService service)
        {
            Console.Out.Write("Enter age: ");
            if (int.TryParse(Console.ReadLine(), out int age))
            {
                int price = service.CalculatePriceForAge(age);
                Console.Out.WriteLine($"Price for age {age}: {price} kr");
            }
            else
            {
                Console.Out.WriteLine("Invalid age. Please enter a number.");
            }
        }

        private static void HandleGroupPrice(Uppgift2TicketService service)
        {
            Console.Out.Write("Enter number of people: ");
            if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
            {
                var ages = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    Console.Out.Write($"Enter age for person {i + 1}: ");
                    if (int.TryParse(Console.ReadLine(), out int age))
                    {
                        ages.Add(age);
                    }
                    else
                    {
                        Console.Out.WriteLine("Invalid age. Please enter a number.");
                        i--;
                    }
                }

                int total = service.CalculateTotalForGroup(ages);
                Console.Out.WriteLine($"Total cost for {count} people: {total} kr");
            }
            else
            {
                Console.Out.WriteLine("Invalid number. Please enter a positive number.");
            }
        }
    }
}
