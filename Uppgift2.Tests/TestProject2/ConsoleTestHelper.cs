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

            try
            {
                var inputReader = new StringReader(input);
                var outputWriter = new StringWriter();

                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                Uppgift2.Uppgift2Program.Main(Array.Empty<string>());

                return outputWriter.ToString();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
    }
}
