using System;
using Xunit;

namespace SolutionSetupTests
{
    public class PersonSalaryTests
    {
        [Theory(DisplayName = "Salary increase under age 30 calculates correctly")]
        [InlineData(25, 1000, 10, 1050)]
        [InlineData(25, 500, 5, 512.5)]
        [InlineData(29, 1000, 33.33, 1166.65)]
        [InlineData(27, 1000, 100, 1500)]
        [InlineData(29, 1000, 40, 1200)]
        [InlineData(25, 1000, 60, 1300)]
        public void IncreaseSalary_UnderAge30_CalculatesCorrectly(int age, double initialSalary, double percentage, double expected)
        {
            // Arrange
            var person = new Person("Test", "Person", age, (decimal)initialSalary);

            // Act
            person.IncreaseSalary((decimal)percentage);

            // Assert
            Assert.Equal((decimal)expected, person.Salary);
        }

        [Theory(DisplayName = "Salary increase at age 30 and above calculates correctly")]
        [InlineData(35, 1000, 50, 1500)]
        [InlineData(45, 1000, 33.333, 1333.33)]
        [InlineData(50, 1000, 2.5, 1025)]
        [InlineData(38, 1000, 0.01, 1000.1)]
        [InlineData(30, 1000, 40, 1400)]
        [InlineData(50, 1000, 60, 1600)]
        public void IncreaseSalary_Age30AndAbove_CalculatesCorrectly(int age, double initialSalary, double percentage, double expected)
        {
            // Arrange
            var person = new Person("Test", "Person", age, (decimal)initialSalary);

            // Act
            person.IncreaseSalary((decimal)percentage);

            // Assert
            Assert.Equal((decimal)expected, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithZeroPercentage_NoChange()
        {
            // Arrange
            var person = new Person("Alice", "Brown", 28, 600m);
            var originalSalary = person.Salary;

            // Act
            person.IncreaseSalary(0);

            // Assert
            Assert.Equal(originalSalary, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithNegativePercentage_ThrowsException()
        {
            // Arrange
            var person = new Person("Charlie", "Davis", 40, 800m);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => person.IncreaseSalary(-10));
            Assert.Equal("Percentage must always be positive!", exception.Message);
        }

        [Fact]
        public void IncreaseSalary_MultipleIncreases_Age30AndAbove_AppliesCorrectly()
        {
            // Arrange
            var person = new Person("Diana", "Evans", 32, 1000m);

            // Act
            person.IncreaseSalary(10);
            person.IncreaseSalary(20);

            // Assert
            // Age 32 (>= 30): First: 1000 + 1000 * (10/100) = 1100, Then: 1100 + 1100 * (20/100) = 1320
            Assert.Equal(1320m, person.Salary);
        }

        [Fact]
        public void Salary_WhenSet_RoundedTo2Decimals()
        {
            // Arrange & Act
            var person = new Person("Grace", "Harris", 26, 500.4567m);

            // Assert
            Assert.Equal(500.46m, person.Salary);
        }

        [Fact]
        public void Salary_WhenSetBelowMinimum_ThrowsException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Person("Henry", "ideal", 30, 400m));
            Assert.Equal($"Salary cannot be less than {Person.DefaultMinimumSalary} dollar!", exception.Message);
        }

        [Fact]
        public void IncreaseSalary_StaysAboveMinimumSalary_Age30AndAbove()
        {
            // Arrange
            var person = new Person("Iris", "Jones", 31, Person.DefaultMinimumSalary);

            // Act
            person.IncreaseSalary(1);

            // Assert
            Assert.True(person.Salary > Person.DefaultMinimumSalary);
        }

        [Fact]
        public void IncreaseSalary_SamePercentage_DifferentAges_UnderVsAge30AndAbove()
        {
            // Arrange
            var personUnder30 = new Person("Michael", "Nelson", 29, 1000m);
            var personAge30Plus = new Person("Oliver", "Parker", 30, 1000m);

            // Act
            personUnder30.IncreaseSalary(20);
            personAge30Plus.IncreaseSalary(20);

            // Assert
            // Age 29 (< 30): 1000 + 1000 * (20/200) = 1100
            Assert.Equal(1100m, personUnder30.Salary);
            // Age 30 (>= 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal(1200m, personAge30Plus.Salary);
        }
    }
}
