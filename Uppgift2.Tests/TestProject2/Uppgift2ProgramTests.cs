using System;
using System.IO;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    [CollectionDefinition("Program Tests", DisableParallelization = true)]

    [Collection("Console Program Tests")]
    public class Uppgift2ProgramTests
    {
        /**
         * Helper method to run the program with specified input line by line and capture the output.
         */
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
        public void UserSelectsExit_DisplaysGoodbye()
        {
            string input = "0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Welcome to the ticket price calculator!", output);
            Assert.Contains("Goodbye!", output);
        }

        [Fact]
        public void UserEntersInvalidChoice_DisplaysError()
        {
            string input = "5\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid choice. Please enter 0, 1, 2, or 3.", output);
        }

        [Fact]
        public void UserCalculatesSinglePersonYouthPrice_ShowsCorrectPrice()
        {
            string input = "1\n15\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Enter age:", output);
            Assert.Contains("Price for age 15: 80 kr", output);
        }

        [Fact]
        public void UserCalculatesSinglePersonStandardPrice_ShowsCorrectPrice()
        {
            string input = "1\n30\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Price for age 30: 120 kr", output);
        }

        [Fact]
        public void UserCalculatesSinglePersonPensionerPrice_ShowsCorrectPrice()
        {
            string input = "1\n70\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Price for age 70: 90 kr", output);
        }

        [Fact]
        public void UserEntersInvalidAge_DisplaysError()
        {
            string input = "1\nabc\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid age. Please enter a number.", output);
        }

        [Fact]
        public void UserCalculatesGroupPrice_ShowsCorrectTotal()
        {
            string input = "2\n3\n10\n30\n70\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Enter number of people:", output);
            Assert.Contains("Enter age for person 1:", output);
            Assert.Contains("Enter age for person 2:", output);
            Assert.Contains("Enter age for person 3:", output);
            Assert.Contains("Total cost for 3 people: 290 kr", output);
        }

        [Fact]
        public void UserEntersInvalidGroupSize_DisplaysError()
        {
            string input = "2\nabc\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid number. Please enter a positive number.", output);
        }

        [Fact]
        public void UserEntersNegativeGroupSize_DisplaysError()
        {
            string input = "2\n-5\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid number. Please enter a positive number.", output);
        }

        [Fact]
        public void UserEntersInvalidAgeInGroup_RetriesToEnterAge()
        {
            string input = "2\n2\ninvalid\n25\n65\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid age. Please enter a number.", output);
            Assert.Contains("Total cost for 2 people: 210 kr", output); // 120 + 90
        }

        [Fact]
        public void UserCalculatesEmptyGroup_DisplaysError()
        {
            string input = "2\n0\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Invalid number. Please enter a positive number.", output);
        }

        [Fact]
        public void UserCalculatesGroupWithBoundaryAges_ShowsCorrectTotal()
        {
            string input = "2\n4\n19\n20\n64\n65\n0\n";
            string output = RunProgramWithInput(input);

            // 80 + 120 + 120 + 90 = 410
            Assert.Contains("Total cost for 4 people: 410 kr", output);
        }

        [Fact]
        public void UserSwitchesBackAndForthBetweenOptions_WorksCorrectly()
        {
            string input = "1\n25\n2\n2\n18\n35\n1\n50\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Price for age 25: 120 kr", output);
            Assert.Contains("Total cost for 2 people: 200 kr", output); // 80 + 120
            Assert.Contains("Price for age 50: 120 kr", output);
        }

        [Fact]
        public void YoungUserUnder20_PaysMostAffordablePrice()
        {
            string input = "1\n0\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Price for age 0: 80 kr", output);
        }

        [Fact]
        public void SeniorUserOver64_PaysPensionerPrice()
        {
            string input = "1\n100\n0\n";
            string output = RunProgramWithInput(input);

            Assert.Contains("Price for age 100: 90 kr", output);
        }

        [Fact]
        public void LargeGroupCalculation_ComputesCorrectly()
        {
            string input = "2\n5\n5\n10\n15\n20\n25\n0\n";
            string output = RunProgramWithInput(input);

            // 80 + 80 + 80 + 120 + 120 = 480
            Assert.Contains("Total cost for 5 people: 480 kr", output);
        }
    }
}
