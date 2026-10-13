namespace SolutionSetupTests
{
    public class PersonTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesPerson()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30);

            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Doe", person.LastName);
            Assert.Equal(30, person.Age);
            Assert.Equal(Person.DefaultMinimumSalary, person.Salary);
        }

        [Fact]
        public void ToString_WithValidPerson_ReturnsFormattedString()
        {
            // Arrange
            var person = new Person("Jane", "Smith", 25);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Contains("Jane", result);
            Assert.Contains("Smith", result);
            Assert.Contains("25", result);
        }

        [Fact]
        public void ToString_WithDifferentAges_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person("Bob", "Johnson", 45);

            // Act
            var result = person.ToString();

            // Assert
            Assert.Contains("Bob Johnson", result);
            Assert.Contains("45", result);
        }

        [Fact]
        public void Constructor_WithDifferentNames_InitializesPerson()
        {
            // Arrange & Act
            var person = new Person("Alice", "Williams", 28);

            // Assert
            Assert.Equal("Alice", person.FirstName);
            Assert.Equal("Williams", person.LastName);
            Assert.Equal(28, person.Age);
        }

        [Fact]
        public void Constructor_WithFirstNameTooShort_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("Jo", "Doe", 30));
            Assert.Equal("First name cannot contain fewer than 3 symbols!", ex.Message);
        }

        [Fact]
        public void Constructor_WithFirstNameNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person(null, "Doe", 30));
            Assert.Equal("First name cannot contain fewer than 3 symbols!", ex.Message);
        }

        [Fact]
        public void Constructor_WithLastNameTooShort_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("John", "Do", 30));
            Assert.Equal("Last name cannot contain fewer than 3 symbols!", ex.Message);
        }

        [Fact]
        public void Constructor_WithLastNameNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("John", null, 30));
            Assert.Equal("Last name cannot contain fewer than 3 symbols!", ex.Message);
        }

        [Fact]
        public void Constructor_WithZeroAge_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("John", "Doe", 0));
            Assert.Equal("Age cannot be zero or a negative integer!", ex.Message);
        }

        [Fact]
        public void Constructor_WithNegativeAge_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("John", "Doe", -5));
            Assert.Equal("Age cannot be zero or a negative integer!", ex.Message);
        }

        [Fact]
        public void Constructor_WithSalaryLessThanMinimum_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Person("John", "Doe", 30, 459.99m));
            Assert.Equal("Salary cannot be less than 460 dollar!", ex.Message);
        }

        [Fact]
        public void Constructor_WithSalaryEqualToMinimum_Succeeds()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30, 460);

            // Assert
            Assert.Equal(460, person.Salary);
        }

        [Fact]
        public void Constructor_WithSalaryGreaterThanMinimum_Succeeds()
        {
            // Arrange & Act
            var person = new Person("John", "Doe", 30, 500);

            // Assert
            Assert.Equal(500, person.Salary);
        }
    }
}
