using System;
using Xunit;

namespace SolutionSetupTests
{
    public class PersonTests
    {
        [Theory(DisplayName = "ToString returns correctly formatted output")]
        [InlineData("John", "Doe", 30, 1000, "John Doe receives 1000.00 dollars.")]
        [InlineData("Jane", "Smith", 25, 1500, "Jane Smith receives 1500.00 dollars.")]
        [InlineData("Bob", "Johnson", 1, 500, "Bob Johnson receives 500.00 dollars.")]
        [InlineData("Anna", "Williams", 100, 2000, "Anna Williams receives 2000.00 dollars.")]
        public void ToString_WithValidPerson_ReturnsFormattedString(string firstName, string lastName, int age, decimal salary, string expected)
        {
            // Arrange
            var person = new Person(firstName, lastName, age, salary);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FirstName_WithValidName_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal("John", person.FirstName);
        }

        [Theory(DisplayName = "Invalid first names throw exception")]
        [InlineData("Jo")]
        [InlineData("A")]
        public void FirstName_WithInvalidValue_ThrowsException(string firstName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person(firstName, "Doe", 30));
        }

        [Fact]
        public void FirstName_WithNullValue_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person(null!, "Doe", 30));
        }

        [Fact]
        public void LastName_WithValidName_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal("Doe", person.LastName);
        }

        [Theory(DisplayName = "Invalid last names throw exception")]
        [InlineData("Do")]
        [InlineData("B")]
        public void LastName_WithInvalidValue_ThrowsException(string lastName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", lastName, 30));
        }

        [Fact]
        public void LastName_WithNullValue_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", null!, 30));
        }

        [Fact]
        public void Age_WithPositiveValue_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal(30, person.Age);
        }

        [Theory(DisplayName = "Invalid ages throw exception")]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(-100)]
        public void Age_WithInvalidValue_ThrowsException(int age)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", "Doe", age));
        }

        [Theory(DisplayName = "Valid salary values are set successfully")]
        [InlineData(1000)]
        [InlineData(460)]
        [InlineData(5000)]
        public void Salary_WithValidValue_SetsSuccessfully(decimal salary)
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30, salary);

            // Assert
            Assert.Equal(salary, person.Salary);
        }

        [Fact]
        public void Salary_WithDefaultValue_SetsToMinimumSalary()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal(Person.DefaultMinimumSalary, person.Salary);
        }

        [Fact]
        public void Salary_BelowMinimumSalary_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", "Doe", 30, 400));
        }

        [Theory(DisplayName = "Salary increases correctly for age 30 and above")]
        [InlineData(10, 1100)]
        [InlineData(50, 1500)]
        [InlineData(0, 1000)]
        public void IncreaseSalary_WithPercentage_Age30_ReceivesCorrectIncrease(decimal percentage, decimal expected)
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000);

            // Act
            person.IncreaseSalary(percentage);

            // Assert
            Assert.Equal(expected, person.Salary);
        }
    }

}