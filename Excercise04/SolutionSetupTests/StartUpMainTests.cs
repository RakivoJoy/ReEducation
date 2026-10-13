namespace SolutionSetupTests
{
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    [Collection("Sequential")] 
    public class StartUpMainTests
    {
        private List<string> CaptureConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                try
                {
                    action.Invoke();
                    var result = writer.ToString();
                    return result.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        private void SimulateStartUpMain(List<string> inputs)
        {
            var originalIn = Console.In;
            using (var reader = new StringReader(string.Join("\n", inputs)))
            {
                Console.SetIn(reader);
                try
                {
                    SolutionSetup.StartUp.Main();
                }
                finally
                {
                    Console.SetIn(originalIn);
                }
            }
        }

        [Fact]
        public void Main_WithSinglePerson_UnderAge30_AppliesHalfBonus()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",                    // Number of persons
                "John Doe 25 1000",     // Name, LastName, Age, Salary
                "10"                    // Bonus percentage
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Age 25 (< 30): 1000 + 1000 * (10/200) = 1050
            Assert.Equal("John Doe receives 1050.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithSinglePerson_Age30AndAbove_AppliesFullBonus()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",                    // Number of persons
                "Jane Smith 35 1000",   // Name, LastName, Age, Salary
                "20"                    // Bonus percentage
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Age 35 (>= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal("Jane Smith receives 1200.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithMultiplePersons_DifferentAges_AppliesBonusCorrectly()
        {
            // Arrange
            var inputs = new List<string>
            {
                "3",
                "Alice Brown 25 1000",
                "Bob Clark 35 1000",
                "Charlie Davis 29 1000",
                "20"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(3, output.Count);
            // Alice 25 (< 30): 1000 + 1000 * (20/200) = 1100
            Assert.Equal("Alice Brown receives 1100.00 dollars.", output[0].Trim());
            // Bob 35 (>= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal("Bob Clark receives 1200.00 dollars.", output[1].Trim());
            // Charlie 29 (< 30): 1000 + 1000 * (20/200) = 1100
            Assert.Equal("Charlie Davis receives 1100.00 dollars.", output[2].Trim());
        }

        [Fact]
        public void Main_WithZeroBonus_SalaryUnchanged()
        {
            // Arrange
            var inputs = new List<string>
            {
                "2",
                "Diana Evans 28 1000",
                "Eve Frank 40 1500",
                "0"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(2, output.Count);
            Assert.Equal("Diana Evans receives 1000.00 dollars.", output[0].Trim());
            Assert.Equal("Eve Frank receives 1500.00 dollars.", output[1].Trim());
        }

        [Fact]
        public void Main_WithInvalidLineCount_ReturnsEarly()
        {
            // Arrange
            var inputs = new List<string>
            {
                "invalid",
                "John Doe 25 1000",
                "20"
            };

            // Act & Assert - Should return early without crashing
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));
            Assert.Empty(output);
        }

        [Fact]
        public void Main_WithMissingBonus_DefaultsToZero()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Frank Garcia 30 1000"
                // No bonus percentage provided
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Bonus defaults to 0
            Assert.Equal("Frank Garcia receives 1000.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithInvalidBonus_DefaultsToZero()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Grace Harris 32 1000",
                "abc"  // Invalid bonus
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Bonus defaults to 0 when parsing fails
            Assert.Equal("Grace Harris receives 1000.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithInvalidPersonData_SkipsInvalidLines()
        {
            // Arrange
            var inputs = new List<string>
            {
                "4",
                "Henry Young 28 1000",
                "Iris invalid age 1000",  // Invalid age
                "Jack Hill 29 invalid",   // Invalid salary
                "Kate Lewis 30 1500",
                "10"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert - Only valid persons should be in output
            Assert.Equal(2, output.Count);
            // Henry 28 (< 30): 1000 + 1000 * (10/200) = 1050
            Assert.Equal("Henry Young receives 1050.00 dollars.", output[0].Trim());
            // Kate 30 (>= 30): 1500 + 1500 * (10/100) = 1650
            Assert.Equal("Kate Lewis receives 1650.00 dollars.", output[1].Trim());
        }

        [Fact]
        public void Main_WithInsufficientFields_SkipsLine()
        {
            // Arrange
            var inputs = new List<string>
            {
                "2",
                "Leo Miller 25",  // Missing salary
                "Mary Johnson 35 2000",
                "10"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Mary 35 (>= 30): 2000 + 2000 * (10/100) = 2200
            Assert.Equal("Mary Johnson receives 2200.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithLargeBonus_CalculatesCorrectly()
        {
            // Arrange
            var inputs = new List<string>
            {
                "2",
                "Noah Patterson 26 1000",
                "Olivia Quinn 40 1000",
                "100"  // 100% bonus
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(2, output.Count);
            // Noah (26, < 30): 1000 + 1000 * (100/200) = 1500
            Assert.Equal("Noah Patterson receives 1500.00 dollars.", output[0].Trim());
            // Olivia (40, >= 30): 1000 + 1000 * (100/100) = 2000
            Assert.Equal("Olivia Quinn receives 2000.00 dollars.", output[1].Trim());
        }

        [Fact]
        public void Main_WithDecimalBonus_NoDecimalSalary()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Peter Roberts 28 1000",
                "10"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Peter (28, < 30): 1000 + 1000 * (10/200) = 1050
            Assert.Equal("Peter Roberts receives 1050.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithBoundaryAge30_ReceivesFullBonus()
        {
            // Arrange
            var inputs = new List<string>
            {
                "2",
                "Rachel Taylor 29 1000",
                "Samuel Young 30 1000",
                "20"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(2, output.Count);
            // Rachel (29, < 30): 1000 + 1000 * (20/200) = 1100
            Assert.Equal("Rachel Taylor receives 1100.00 dollars.", output[0].Trim());
            // Samuel (30, >= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal("Samuel Young receives 1200.00 dollars.", output[1].Trim());
        }

        [Fact]
        public void Main_WithRoundingNeeded_RoundsTo2Decimals()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Tina White 35 1000",
                "20"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Tina (35, >= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal("Tina White receives 1200.00 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithMinimumSalaryEnforcement_RespectsBoundary()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Uma Brown 28 460",  // Minimum salary
                "5"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Single(output);
            // Uma (28, < 30): 460 + 460 * (5/200) = 471.50
            Assert.Equal("Uma Brown receives 471.50 dollars.", output[0].Trim());
        }

        [Fact]
        public void Main_WithEmptyInput_HandlesGracefully()
        {
            // Arrange
            var inputs = new List<string>
            {
                "0",  // No persons
                "10"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Empty(output);
        }

        [Fact]
        public void Main_WithNegativeBonus_HandlesGracefully()
        {
            // Arrange
            var inputs = new List<string>
            {
                "1",
                "Victor Green 25 1000",
                "-10"
            };

            // Act - Should not throw exception, just skip bonus application
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert - Should still output the person (bonus was invalid, so not applied)
            Assert.NotEmpty(output);
            // Victor (25, < 30) with -10 bonus skipped: 1000 stays as 1000
            Assert.Equal("Victor Green receives 1000.00 dollars.", output[output.Count - 1].Trim());
        }

        [Fact]
        public void Main_ProvidedExample_Scenario()
        {
            // Arrange - Based on the provided example with adjusted bonus
            var inputs = new List<string>
            {
                "2",
                "Bert Lewinsson 27 600",
                "Anna Hamren 44 1000",
                "20"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(2, output.Count);
            // Bert (27, < 30): 600 + 600 * (20/200) = 660
            Assert.Equal("Bert Lewinsson receives 660.00 dollars.", output[0].Trim());
            // Anna (44, >= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal("Anna Hamren receives 1200.00 dollars.", output[1].Trim());
        }

        [Fact]
        public void Main_MultiplePersons_ApplyBonus()
        {
            // Arrange
            var inputs = new List<string>
            {
                "3",
                "Zoe Adams 22 800",
                "Bob Brown 35 1200",
                "Alice Clark 28 900",
                "10"
            };

            // Act
            var output = CaptureConsoleOutput(() => SimulateStartUpMain(inputs));

            // Assert
            Assert.Equal(3, output.Count);
            // Zoe (22, < 30): 800 + 800 * (10/200) = 840
            Assert.Equal("Zoe Adams receives 840.00 dollars.", output[0].Trim());
            // Bob (35, >= 30): 1200 + 1200 * (10/100) = 1320
            Assert.Equal("Bob Brown receives 1320.00 dollars.", output[1].Trim());
            // Alice (28, < 30): 900 + 900 * (10/200) = 945
            Assert.Equal("Alice Clark receives 945.00 dollars.", output[2].Trim());
        }
    }
}
