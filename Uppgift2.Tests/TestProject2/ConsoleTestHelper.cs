using System;
using System.IO;

namespace Uppgift2.Tests
{
    public static class ConsoleTestHelper
    {
        // Centralized helper that runs the console program with provided input
        // and returns the captured output as a string.
        public static string RunProgramWithInput(string input)
        {
            var originalIn = Console.In;
            var originalOut = Console.Out;
            StringReader inputReader = null;
            StringWriter outputWriter = null;

            try
            {
                inputReader = new StringReader(input);
                outputWriter = new StringWriter();

                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                try
                {
                    Uppgift2.Uppgift2Program.Main(Array.Empty<string>());
                }
                catch (Exception ex)
                {
                    // Log the exception and rethrow to let the test handle it
                    Console.Out.WriteLine($"Program threw an exception: {ex.GetType().Name}: {ex.Message}");
                    throw;
                }

                return outputWriter.ToString();
            }
            finally
            {
                try
                {
                    Console.SetIn(originalIn);
                    Console.SetOut(originalOut);
                }
                catch (Exception ex)
                {
                    // Ensure we don't lose the original exception by exception handling in finally
                    Console.Error.WriteLine($"Error restoring console streams: {ex.Message}");
                }
                finally
                {
                    // Dispose of resources
                    inputReader?.Dispose();
                    outputWriter?.Dispose();
                }
            }
        }
    }
}
