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
                SetupConsoleForTesting(input, out inputReader, out outputWriter);
                RunProgramSafely();
                return outputWriter.ToString();
            }
            finally
            {
                RestoreConsoleStreams(originalIn, originalOut, inputReader, outputWriter);
            }
        }

        private static void SetupConsoleForTesting(string input, out StringReader inputReader, out StringWriter outputWriter)
        {
            try
            {
                inputReader = new StringReader(input);
                outputWriter = new StringWriter();

                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error setting up test console: {ex.Message}");
                throw;
            }
        }

        private static void RunProgramSafely()
        {
            try
            {
                Uppgift2.Uppgift2Program.Main(Array.Empty<string>());
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Program threw an exception: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }

        private static void RestoreConsoleStreams(TextReader originalIn, TextWriter originalOut, StringReader inputReader, StringWriter outputWriter)
        {
            try
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error restoring console streams: {ex.Message}");
            }
            finally
            {
                DisposeResources(inputReader, outputWriter);
            }
        }

        private static void DisposeResources(StringReader inputReader, StringWriter outputWriter)
        {
            try
            {
                inputReader?.Dispose();
                outputWriter?.Dispose();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error disposing resources: {ex.Message}");
            }
        }
    }
}
