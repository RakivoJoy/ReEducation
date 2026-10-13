namespace SolutionSetupTests
{
    public class PersonSalaryTests
    {
        [Fact]
        public void IncreaseSalary_WithPositivePercentage_UnderAge30_IncreasesCorrectly()
        {
            // Arrange
            var person = new Person("John", "Doe", 25, 1000m);
            var originalSalary = person.Salary;

            // Act
            person.IncreaseSalary(10);

            // Assert
            var expectedSalary = originalSalary * 1.1m;
            Assert.Equal(expectedSalary, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithSmallPercentage_UnderAge30_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Jane", "Smith", 25, 500m);

            // Act
            person.IncreaseSalary(5);

            // Assert
            Assert.Equal(525m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithLargePercentage_Age30AndAbove_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Bob", "Johnson", 35, 1000m);

            // Act
            person.IncreaseSalary(50);

            // Assert
            // Age 35 (>= 30): 1000 + 1000 * (50/200) = 1000 + 250 = 1250
            Assert.Equal(1250m, person.Salary);
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
            // Age 32 (>= 30): First: 1000 + 1000 * (10/200) = 1050, Then: 1050 + 1050 * (20/200) = 1155
            Assert.Equal(1155m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithSmallDecimalIncrease_UnderAge30_RoundedTo2Decimals()
        {
            // Arrange
            var person = new Person("Eve", "Frank", 29, 1000m);

            // Act
            person.IncreaseSalary(33.33m);

            // Assert
            // Age 29 (< 30): 1000 + 1000 * (33.33/100) = 1333.30
            Assert.Equal(1333.30m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_WithFractionalResult_Age30AndAbove_RoundsTo2Decimals()
        {
            // Arrange
            var person = new Person("Frank", "Garcia", 45, 1000m);

            // Act
            person.IncreaseSalary(33.333m);

            // Assert
            // Age 45 (>= 30): 1000 + 1000 * (33.333/200) = 1166.665 = 1166.66 (rounded down)
            Assert.Equal(1166.66m, person.Salary);
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
        public void IncreaseSalary_WithDecimalPercentage_Age30AndAbove_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Jack", "King", 50, 1000m);

            // Act
            person.IncreaseSalary(2.5m);

            // Assert
            // Age 50 (>= 30): 1000 + 1000 * (2.5/200) = 1012.50
            Assert.Equal(1012.5m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_VeryLargePercentage_UnderAge30_CalculatesCorrectly()
        {
            // Arrange
            var person = new Person("Kate", "Lee", 27, 1000m);

            // Act
            person.IncreaseSalary(100);

            // Assert
            // Age 27 (< 30): 1000 + 1000 * (100/100) = 2000
            Assert.Equal(2000m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_MinimalPositivePercentage_Age30AndAbove_Increases()
        {
            // Arrange
            var person = new Person("Leo", "Martinez", 38, 1000m);

            // Act
            person.IncreaseSalary(0.01m);

            // Assert
            // Age 38 (>= 30): 1000 + 1000 * (0.01/200) = 1000.05
            Assert.Equal(1000.05m, person.Salary);
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
            // Age 29 (< 30): 1000 + 1000 * (20/100) = 1200
            Assert.Equal(1200m, personUnder30.Salary);
            // Age 30 (>= 30): 1000 + 1000 * (20/200) = 1100
            Assert.Equal(1100m, personAge30Plus.Salary);
        }

        [Fact]
        public void IncreaseSalary_ExactlyAge30_ReceivesHalfIncrease()
        {
            // Arrange
            var person = new Person("Patricia", "Quinn", 30, 1000m);

            // Act
            person.IncreaseSalary(40);

            // Assert
            // Age 30 (>= 30): 1000 + 1000 * (40/200) = 1200
            Assert.Equal(1200m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_Age29_ReceivesFullIncrease()
        {
            // Arrange
            var person = new Person("Quinn", "Rodriguez", 29, 1000m);

            // Act
            person.IncreaseSalary(40);

            // Assert
            // Age 29 (< 30): 1000 + 1000 * (40/100) = 1400
            Assert.Equal(1400m, person.Salary);
        }

        [Fact]
        public void IncreaseSalary_CompareYoungVsOldWorker_SameInvestment()
        {
            // Arrange
            var youngWorker = new Person("Tina", "Taylor", 25, 1000m);
            var seniorWorker = new Person("Uma", "Underwood", 50, 1000m);

            // Act
            youngWorker.IncreaseSalary(60);
            seniorWorker.IncreaseSalary(60);

            // Assert
            // Age 25 (< 30): 1000 + 1000 * (60/100) = 1600
            Assert.Equal(1600m, youngWorker.Salary);
            // Age 50 (>= 30): 1000 + 1000 * (60/200) = 1300
            Assert.Equal(1300m, seniorWorker.Salary);
        }
    }
}
