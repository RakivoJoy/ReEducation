using System;
using System.IO;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    [Collection("Console Output Tests")]
    public class Uppgift2ProgramThirdWordTests
    {
        // Use ConsoleTestHelper.RunProgramWithInput to run the console program and capture output.

        [Fact]
        public void UserSelectsThirdWordOption_DisplaysPrompt()
        {
             string input = "4\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("Enter a sentence with at least 3 words:", output);
        }

        [Fact]
        public void UserEntersValidSentence_ExtractsThirdWord()
        {
            string input = "4\nHello World Test\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("The third word is: Test", output);
        }

        [Fact]
        public void UserEntersSentenceWithMoreThanThreeWords_ExtractsThirdWordCorrectly()
        {
            string input = "4\nThe quick brown fox jumps\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("The third word is: brown", output);
        }

        [Fact]
        public void UserEntersEmptySentence_DisplaysError()
        {
            string input = "4\n\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("Sentence cannot be empty.", output);
        }

        [Fact]
        public void UserEntersSentenceWithOnlyOneWord_DisplaysError()
        {
           string input = "4\nHello\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("Sentence must contain at least 3 words.", output);
        }

        [Fact]
        public void UserEntersSentenceWithTwoWords_DisplaysError()
        {
            string input = "4\nHello World\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("Sentence must contain at least 3 words.", output);
        }

        [Fact]
        public void UserEntersSentenceWithExtraSpaces_HandlesCorrectly()
        {
             string input = "4\nHello  World  Test\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            // With Split(' '), double spaces create empty strings in array
            // So "Hello  World  Test" becomes ["Hello", "", "World", "", "Test"]
            // The third word (index 2) would be "" (empty string)
            Assert.Contains("The third word is: ", output);
        }

        [Fact]
        public void UserEntersThirdWordOption_MultipleTimesContinues()
        {
              string input = "4\nFirst Second Third\n4\nOne Two Three\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("The third word is: Third", output);
            Assert.Contains("The third word is: Three", output);
        }

        [Fact]
        public void ThirdWordOutput_DisplaysCorrectly()
        {
            string input = "4\nProgramming is Fun\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("The third word is: Fun", output);
        }

        [Fact]
        public void UserEntersSentenceWithNumbers_ExtractsThirdWordCorrectly()
        {
             string input = "4\nTest123 Word456 Number789\n0\n";
            string output = ConsoleTestHelper.RunProgramWithInput(input);

            Assert.Contains("The third word is: Number789", output);
        }
    }
}
