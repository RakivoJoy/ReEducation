using System;
using System.IO;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
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
            // TODO: Implement test
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersSimpleWord_RepeatsCorrectly()
        {
            // TODO: Implement test - "Hello" should produce "1. Hello, 2. Hello, 3. Hello,..."
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersText_FormatsWithNumbers()
        {
            // TODO: Implement test - Verify numbers 1-10 are prefixed correctly
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersText_SeparatesWithCommas()
        {
            // TODO: Implement test - Verify output uses commas between repetitions
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersEmptyText_DisplaysError()
        {
            // TODO: Implement test - Empty or whitespace input should show error
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersWhitespaceOnly_DisplaysError()
        {
            // TODO: Implement test - Spaces/tabs only should show error
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersTextWithSpecialCharacters_RepeatsCorrectly()
        {
            // TODO: Implement test - "!@#" or "abc123" should work
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersTextWithSpaces_PreservesSpaces()
        {
            // TODO: Implement test - "Hello World" should maintain spaces in output
            throw new NotImplementedException();
        }

        [Fact]
        public void RepeatTextOutput_HasNoLineBreaks()
        {
            // TODO: Implement test - All 10 repetitions should be on one line (no \n between them)
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersLongText_StillRepeats()
        {
            // TODO: Implement test - Long strings should repeat all 10 times
            throw new NotImplementedException();
        }

        [Fact]
        public void UserChoosesOption3MultipleTimes_WorksEachTime()
        {
            // TODO: Implement test - User can repeat the operation multiple times in one session
            throw new NotImplementedException();
        }

        [Fact]
        public void UserEntersNumericText_RepeatsNumersCorrectly()
        {
            // TODO: Implement test - "123" as input should repeat as "1. 123, 2. 123..."
            throw new NotImplementedException();
        }

        [Fact]
        public void OutputFormat_StartsWithOne()
        {
            // TODO: Implement test - Output should start with "1. " not "0. "
            throw new NotImplementedException();
        }

        [Fact]
        public void OutputFormat_EndsWithTen()
        {
            // TODO: Implement test - Output should end with "10. " and no trailing comma
            throw new NotImplementedException();
        }
    }
}
