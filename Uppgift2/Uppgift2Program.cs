using System;
using System.Collections.Generic;
using System.IO;

namespace Uppgift2
{
    // Main program shell for the exercise "Flow with loops and string manipulation".

    public static class Uppgift2Program
    {
        // Entry point used by the final application.
        public static void Main(string[] args)
        {
            try
            {
                var service = new Uppgift2TicketService();
                bool running = true;

                Console.Out.WriteLine(
                    "Welcome to your next assignment with ticket prices, repeats and third words!");
                Console.Out.WriteLine(
                    "Use the menu below to navigate. Input numbers depending on what you want to do.\n");

                while (running)
                {
                    try
                    {
                        Console.Out.WriteLine("\n--- Menu ---");
                        Console.Out.WriteLine("0. Exit");
                        Console.Out.WriteLine("1. Calculate single person ticket price");
                        Console.Out.WriteLine("2. Calculate group ticket price");
                        Console.Out.WriteLine("3. Repeat text ten times");
                        Console.Out.WriteLine("4. Extract third word from sentence");
                        Console.Out.Write("Enter your choice: ");

                        string choice = Console.ReadLine();

                        // Exit if no input is available (e.g. when running in a test environment)
                        if (choice == null)
                        {
                            running = false;
                            break;
                        }

                        switch (choice)
                        {
                            case "1":
                                HandleSinglePerson(service);
                                break;
                            case "2":
                                HandleGroupPrice(service);
                                break;
                            case "3":
                                HandleRepeatText();
                                break;
                            case "4":
                                HandleThirdWord();
                                break;
                            case "0":
                                running = false;
                                Console.Out.WriteLine("Goodbye!");
                                break;
                            default:
                                Console.Out.WriteLine("Invalid choice. Please enter 0, 1, 2, 3, or 4.");
                                break;
                        }
                    }
                    catch (IOException ioEx)
                    {
                        Console.Out.WriteLine($"I/O Error: {ioEx.Message}");
                        running = false;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine($"Unexpected error in menu loop: {ex.GetType().Name}: {ex.Message}");
                        running = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Fatal error in application: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void HandleSinglePerson(Uppgift2TicketService service)
        {
            try
            {
                Console.Out.Write("Enter age: ");
                string input = Console.ReadLine();

                if (input == null)
                {
                    Console.Out.WriteLine("No input provided.");
                    return;
                }

                // Safe parsing: TryParse prevents exceptions on bad input and returns false instead.
                if (int.TryParse(input, out int age))
                {
                    DisplayTicketPrice(service, age);
                }
                else
                {
                    Console.Out.WriteLine("Invalid age. Please enter a number.");
                }
            }
            catch (IOException ioEx)
            {
                Console.Out.WriteLine($"I/O error reading input: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Error in single person handler: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void DisplayTicketPrice(Uppgift2TicketService service, int age)
        {
            try
            {
                int price = service.CalculatePriceForAge(age);
                Console.Out.WriteLine($"Price for age {age}: {price} kr");
            }
            catch (ArgumentException ex)
            {
                Console.Out.WriteLine($"Validation error: {ex.Message}");
            }
        }

        private static void HandleGroupPrice(Uppgift2TicketService service)
        {
            try
            {
                Console.Out.Write("Enter number of people: ");
                string countInput = Console.ReadLine();

                if (countInput == null)
                {
                    Console.Out.WriteLine("No input provided.");
                    return;
                }

                // If parsing fails, the right-hand condition is not evaluated.
                if (int.TryParse(countInput, out int count) && count > 0)
                {
                    var ages = CollectAgesFromUser(count);
                    CalculateAndDisplayGroupTotal(service, ages, count);
                }
                else
                {
                    Console.Out.WriteLine("Invalid number. Please enter a positive number.");
                }
            }
            catch (IOException ioEx)
            {
                Console.Out.WriteLine($"I/O error reading input: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Error in group price handler: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static List<int> CollectAgesFromUser(int count)
        {
            var ages = new List<int>();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    Console.Out.Write($"Enter age for person {i + 1}: ");
                    string ageInput = Console.ReadLine();

                    if (ageInput == null)
                    {
                        Console.Out.WriteLine("No input provided.");
                        i--;
                        continue;
                    }

                    if (int.TryParse(ageInput, out int age))
                    {
                        ages.Add(age);
                    }
                    else
                    {
                        Console.Out.WriteLine("Invalid age. Please enter a number.");
                                // Decrement 'i' to retry the same person index on invalid input.
                                // Without this, an invalid entry would still count toward the total loop iterations.
                        i--;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.Out.WriteLine($"Age validation error: {ex.Message}");
                    i--;
                }
            }
            return ages;
        }

        private static void CalculateAndDisplayGroupTotal(Uppgift2TicketService service, List<int> ages, int count)
        {
            try
            {
                int total = service.CalculateTotalForGroup(ages);
                Console.Out.WriteLine($"Total cost for {count} people: {total} kr");
            }
            catch (ArgumentNullException ex)
            {
                Console.Out.WriteLine($"Group calculation error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.Out.WriteLine($"Calculation error: {ex.Message}");
            }
        }

        private static void HandleRepeatText()
        {
            try
            {
                Console.Out.Write("Enter text to repeat: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Out.WriteLine("Text cannot be empty.");
                    return;
                }

                DisplayRepeatedText(input);
            }
            catch (IOException ioEx)
            {
                Console.Out.WriteLine($"I/O error reading input: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Error in repeat text handler: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void DisplayRepeatedText(string input)
        {
            try
            {
                Console.Out.Write("Output: ");
                for (int i = 1; i <= 10; i++)
                {
                    Console.Out.Write($"{i}. {input}");
                    // Tricky formatting: add a comma between items but avoid a trailing comma after the last item
                    if (i < 10)
                        Console.Out.Write(", ");
                }
                Console.Out.WriteLine();
            }
            catch (IOException ioEx)
            {
                Console.Out.WriteLine($"I/O error during output: {ioEx.Message}");
            }
        }

        private static void HandleThirdWord()
        {
            try
            {
                Console.Out.Write("Enter a sentence with at least 3 words: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Out.WriteLine("Sentence cannot be empty.");
                    return;
                }

                ExtractAndDisplayThirdWord(input);
            }
            catch (IOException ioEx)
            {
                Console.Out.WriteLine($"I/O error reading input: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Error in third word handler: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void ExtractAndDisplayThirdWord(string input)
        {
            try
            {
                // Split on a single space character. Note: consecutive spaces produce empty entries.
                // TODO: to ignore multiple spaces, consider using input.Split(' ', StringSplitOptions.RemoveEmptyEntries).
                string[] words = input.Split(' ');

                if (words.Length < 3)
                {
                    Console.Out.WriteLine("Sentence must contain at least 3 words.");
                    return;
                }

                string thirdWord = words[2];
                Console.Out.WriteLine($"The third word is: {thirdWord}");
            }
            catch (ArgumentException ex)
            {
                Console.Out.WriteLine($"Error processing sentence: {ex.Message}");
            }
        }
    }
}
