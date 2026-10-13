using Xunit;
namespace SolutionSetupTests
{
    public class PersonTests
    {
        [Fact]
        public void ToString_WithValidPerson_ReturnsFormattedString()
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Equal("John Doe is 30 years old.", result);
        }

        [Fact]
        public void ToString_WithDifferentNames_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person("Jane", "Smith", 25, 1500);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Equal("Jane Smith is 25 years old.", result);
        }

        [Fact]
        public void ToString_WithMinimumValidAge_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person("Bob", "Johnson", 1, 500);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Equal("Bob Johnson is 1 years old.", result);
        }

        [Fact]
        public void ToString_WithLargeAge_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person("Anna", "Williams", 100, 2000);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Equal("Anna Williams is 100 years old.", result);
        }

        [Fact]
        public void FirstName_WithValidName_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal("John", person.FirstName);
        }

        [Fact]
        public void FirstName_WithNameLessThan3Chars_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("Jo", "Doe", 30));
        }

        [Fact]
        public void FirstName_WithNullValue_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person(null, "Doe", 30));
        }

        [Fact]
        public void LastName_WithValidName_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void LastName_WithNameLessThan3Chars_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", "Do", 30));
        }

        [Fact]
        public void LastName_WithNullValue_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", null, 30));
        }

        [Fact]
        public void Age_WithPositiveValue_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal(30, person.Age);
        }

        [Fact]
        public void Age_WithZero_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", "Doe", 0));
        }

        [Fact]
        public void Age_WithNegativeValue_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person("John", "Doe", -5));
        }

        [Fact]
        public void Salary_WithValidValue_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30, 1000);

            // Assert
            Assert.Equal(1000m, person.Salary);
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

        [Fact]
        public void Salary_EqualToMinimumSalary_SetsSuccessfully()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30, 460);

            // Assert
            Assert.Equal(460m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithPositivePercentage_IncreasesCorrectly()
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000);

            // Act
            person.IncreaseSalary(10);

            // Assert
            Assert.Equal(1100m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithZeroPercentage_SalaryUnchanged()
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => person.IncreaseSalary(0));
        }

        [Fact]
        public void IncreaseSalary_WithHighPercentage_IncreasesCorrectly()
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000);

            // Act
            person.IncreaseSalary(50);

            // Assert
            Assert.Equal(1500m, person.Salary);
        }
    }

}