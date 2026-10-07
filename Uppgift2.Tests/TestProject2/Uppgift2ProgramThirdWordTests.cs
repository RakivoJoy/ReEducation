using System;
using System.IO;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    [Collection("Console Output Tests")]
    public class Uppgift2ProgramThirdWordTests
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
        public void UserSelectsThirdWordOption_DisplaysPrompt()
        {
            // TODO: Verify that selecting option 4 displays the prompt for entering sentence
            string input = "4\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersValidSentence_ExtractsThirdWord()
        {
            // TODO: Verify that a valid 3-word sentence returns the third word correctly
            string input = "4\nHello World Test\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersSentenceWithMoreThanThreeWords_ExtractsThirdWordCorrectly()
        {
            // TODO: Verify extraction works with more than 3 words
            string input = "4\nThe quick brown fox jumps\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersEmptySentence_DisplaysError()
        {
            // TODO: Handle empty input validation
            string input = "4\n\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersSentenceWithOnlyOneWord_DisplaysError()
        {
            // TODO: Handle sentence with fewer than 3 words
            string input = "4\nHello\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersSentenceWithTwoWords_DisplaysError()
        {
            // TODO: Handle sentence with fewer than 3 words
            string input = "4\nHello World\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersSentenceWithExtraSpaces_HandlesCorrectly()
        {
            // TODO: Verify handling of multiple spaces between words
            string input = "4\nHello  World  Test\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersThirdWordOption_MultipleTimesContinues()
        {
            // TODO: Verify that option 4 works multiple times in same session
            string input = "4\nFirst Second Third\n4\nOne Two Three\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void ThirdWordOutput_DisplaysCorrectly()
        {
            // TODO: Verify output format is clear and correct
            string input = "4\nProgramming is Fun\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }

        [Fact]
        public void UserEntersSentenceWithNumbers_ExtractsThirdWordCorrectly()
        {
            // TODO: Verify handling of words that contain numbers
            string input = "4\nTest123 Word456 Number789\n0\n";
            string output = RunProgramWithInput(input);

            // Assert
        }
    }
}
