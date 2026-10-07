using System;
using System.IO;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    [CollectionDefinition("Console Output Tests", DisableParallelization = true)]
    public class ConsoleOutputCollection
    {
        // This collection definition disables parallel execution for tests in it
    }

    [Collection("Console Output Tests")]
    public class Uppgift2ProgramRepeatTextTests
    {
        private string RunProgramWithInput(string input)
        {
            var originalIn = Console.In;
            var originalOut = Console.Out;

            try
            {
                var inputReader = new StringReader(input);
                var outputWriter = new StringWriter();

                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                Uppgift2Program.Main(Array.Empty<string>());

                return outputWriter.ToString();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void UserSelectsRepeatTextOption_DisplaysPrompt()
        {
            string input = "3\n\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Enter text to repeat:", output);
        }

        [Fact]
        public void UserEntersText_RepeatsItTenTimes()
        {
            string input = "3\nTest\n0\n";
            string output = RunProgramWithInput(input);

            for (int i = 1; i <= 10; i++)
            {
                Assert.Contains($"{i}. Test", output);
            }
        }

        [Fact]
        public void UserEntersSimpleWord_RepeatsCorrectly()
        {
            string input = "3\nHello\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. Hello, 2. Hello, 3. Hello, 4. Hello, 5. Hello, 6. Hello, 7. Hello, 8. Hello, 9. Hello, 10. Hello", output);
        }

        [Fact]
        public void UserEntersText_FormatsWithNumbers()
        {
            string input = "3\nWord\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. Word", output);
            Assert.Contains("5. Word", output);
            Assert.Contains("10. Word", output);
        }

        [Fact]
        public void UserEntersText_SeparatesWithCommas()
        {
            string input = "3\nA\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. A, 2. A", output);
            Assert.Contains("9. A, 10. A", output);
            Assert.DoesNotContain("10. A,", output);
        }

        [Fact]
        public void UserEntersEmptyText_DisplaysError()
        {
            string input = "3\n\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Text cannot be empty.", output);
        }

        [Fact]
        public void UserEntersWhitespaceOnly_DisplaysError()
        {
            string input = "3\n   \n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Text cannot be empty.", output);
        }

        [Fact]
        public void UserEntersTextWithSpecialCharacters_RepeatsCorrectly()
        {
            string input = "3\n!@#$%\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. !@#$%, 2. !@#$%", output);
            Assert.Contains("10. !@#$%", output);
        }

        [Fact]
        public void UserEntersTextWithSpaces_PreservesSpaces()
        {
            string input = "3\nHello World\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. Hello World, 2. Hello World", output);
        }

        [Fact]
        public void RepeatTextOutput_HasNoLineBreaks()
        {
            string input = "3\nText\n0\n";
            string output = RunProgramWithInput(input);

            int outputStart = output.IndexOf("Output:");
            int nextMenuPos = output.IndexOf("--- Menu ---", outputStart);
            string repeatSection = output.Substring(outputStart, nextMenuPos - outputStart);

            int lineBreakCount = repeatSection.Count(c => c == '\n');
            Assert.True(lineBreakCount <= 2, "Output should be on single line with max 2 newline at end");
        }

        [Fact]
        public void UserEntersLongText_StillRepeats()
        {
            string input = "3\nThisIsAVeryLongTextString\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. ThisIsAVeryLongTextString", output);
            Assert.Contains("10. ThisIsAVeryLongTextString", output);
        }

        [Fact]
        public void UserChoosesOption3MultipleTimes_WorksEachTime()
        {
            string input = "3\nFirst\n3\nSecond\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. First, 2. First", output);
            Assert.Contains("1. Second, 2. Second", output);
        }

        [Fact]
        public void UserEntersNumericText_RepeatsNumersCorrectly()
        {
            string input = "3\n123\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. 123, 2. 123, 3. 123", output);
            Assert.Contains("10. 123", output);
        }

        [Fact]
        public void OutputFormat_StartsWithOne()
        {
            string input = "3\nTest\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("1. Test", output);
            Assert.DoesNotContain(" 0. Test", output);
        }

        [Fact]
        public void OutputFormat_EndsWithTen()
        {
            string input = "3\nX\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("10. X", output);
            Assert.DoesNotContain("11. X", output);
        }
    }
}
