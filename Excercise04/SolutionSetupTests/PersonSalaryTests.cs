namespace SolutionSetupTests
{
    public class PersonSalaryTests
    {
        [Fact]
        public void IncreaseSalary_WithPositivePercentage_IncreasesCorrectly()
        {
            // Arrange
            var person = new Person("John", "Doe", 30, 1000m);
            var originalSalary = person.Salary;

            // Act
            person.IncreaseSalary(10);

            // Assert
            var expectedSalary = originalSalary * 1.1m;
            Assert.Equal(expectedSalary, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithSmallPercentage_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Jane", "Smith", 25, 500m);

            // Act
            person.IncreaseSalary(5);

            // Assert
            Assert.Equal(525m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithLargePercentage_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Bob", "Johnson", 35, 1000m);

            // Act
            person.IncreaseSalary(50);

            // Assert
            Assert.Equal(1500m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithZeroPercentage_ThrowsException()
        {
            // Arrange
            var person = new Person("Alice", "Brown", 28, 600m);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => person.IncreaseSalary(0));
            Assert.Equal("Percentage must always be positive!", exception.Message);
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
        public void IncreaseSalary_MultipleIncreases_AppliesCorrectly()
        {
            // Arrange
            var person = new Person("Diana", "Evans", 32, 1000m);

            // Act
            person.IncreaseSalary(10);
            person.IncreaseSalary(20);

            // Assert
            var expectedSalary = 1000m * 1.1m * 1.2m;
            Assert.Equal(expectedSalary, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithSmallDecimalIncrease_RoundedTo2Decimals()
        {
            // Arrange
            var person = new Person("Eve", "Frank", 29, 1000m);

            // Act
            person.IncreaseSalary(33.33m);

            // Assert
            Assert.Equal(1333.30m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithFractionalResult_RoundsTo2Decimals()
        {
            // Arrange
            var person = new Person("Frank", "Garcia", 45, 1000m);

            // Act
            person.IncreaseSalary(33.333m);

            // Assert
            // 1000 * 1.33333 = 1333.33, rounded to 2 decimals = 1333.33
            Assert.Equal(1333.33m, person.Salary);
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
        public void IncreaseSalary_StaysAboveMinimumSalary_AfterDecrease()
        {
            // Arrange
            var person = new Person("Iris", "Jones", 31, Person.DefaultMinimumSalary);

            // Act
            person.IncreaseSalary(1);

            // Assert
            Assert.True(person.Salary > Person.DefaultMinimumSalary);
        }

        [Fact]
        public void IncreaseSalary_WithDecimalPercentage_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Jack", "King", 50, 1000m);

            // Act
            person.IncreaseSalary(2.5m);

            // Assert
            Assert.Equal(1025m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_VeryLargePercentage_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Kate", "Lee", 27, 1000m);

            // Act
            person.IncreaseSalary(100);

            // Assert
            Assert.Equal(2000m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_MinimalPositivePercentage_Increases()
        {
            // Arrange
            var person = new Person("Leo", "Martinez", 38, 1000m);

            // Act
            person.IncreaseSalary(0.01m);

            // Assert
            var expectedSalary = 1000m * 1.0001m;
            Assert.Equal(Math.Round(expectedSalary, 2), person.Salary);
        }
    }
}
