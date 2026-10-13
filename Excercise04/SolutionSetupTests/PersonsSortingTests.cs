namespace SolutionSetupTests
{
    public class PersonsSortingTests
    {
        [Fact]
        public void PersonList_CanBeCreatedWithMultiplePersons()
        {
            // Arrange
            var persons = new List<Person>();

            // Act
            persons.Add(new Person("John", "Doe", 30));
            persons.Add(new Person("Jane", "Smith", 25));

            // Assert
            Assert.Equal(2, persons.Count);
            Assert.Equal("John", persons[0].FirstName);
            Assert.Equal("Jane", persons[1].FirstName);
        }

        [Fact]
        public void PersonList_CanBeSortedByFirstName()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person("Zoe", "Adams", 28),
                new Person("Alice", "Brown", 30),
                new Person("Bob", "Clark", 25)
            };

            // Act
            var sorted = persons.OrderBy(p => p.FirstName).ToList();

            // Assert
            Assert.Equal(3, sorted.Count);
            Assert.Equal("Alice", sorted[0].FirstName);
            Assert.Equal("Bob", sorted[1].FirstName);
            Assert.Equal("Zoe", sorted[2].FirstName);
        }

        [Fact]
        public void PersonList_CanBeSortedByFirstNameThenAge()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person("John", "Doe", 25),
                new Person("John", "Smith", 30),
                new Person("Alice", "Brown", 28)
            };

            // Act
            var sorted = persons.OrderBy(p => p.FirstName).ThenBy(p => p.Age).ToList();

            // Assert
            Assert.Equal(3, sorted.Count);
            Assert.Equal("Alice", sorted[0].FirstName);
            Assert.Equal(28, sorted[0].Age);
            Assert.Equal("John", sorted[1].FirstName);
            Assert.Equal(25, sorted[1].Age);
            Assert.Equal("John", sorted[2].FirstName);
            Assert.Equal(30, sorted[2].Age);
        }

        [Fact]
        public void PersonList_CanBeConvertedToStrings()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person("John", "Doe", 30),
                new Person("Jane", "Smith", 25)
            };

            // Act
            var results = persons.Select(p => p.ToString()).ToList();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal("John Doe receives 460.00 dollars.", results[0]);
            Assert.Equal("Jane Smith receives 460.00 dollars.", results[1]);
        }

        [Fact]
        public void PersonList_SortedByFirstNameThenAge_OutputsCorrectOrder()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person("Zoe", "Adams", 35),
                new Person("Alice", "Brown", 28),
                new Person("Alice", "Clark", 22),
                new Person("Bob", "Davis", 30)
            };

            // Act
            var sorted = persons.OrderBy(p => p.FirstName).ThenBy(p => p.Age).ToList();
            var results = sorted.Select(p => p.ToString()).ToList();

            // Assert
            Assert.Equal(4, results.Count);
            Assert.Equal("Alice Clark receives 460.00 dollars.", results[0]);
            Assert.Equal("Alice Brown receives 460.00 dollars.", results[1]);
            Assert.Equal("Bob Davis receives 460.00 dollars.", results[2]);
            Assert.Equal("Zoe Adams receives 460.00 dollars.", results[3]);
        }

        [Fact]
        public void ParseInput_WithValidNumberOfLines_CreatesCorrectNumberOfPersons()
        {
            // Arrange
            var inputLines = new[] { "2", "John Doe 30", "Jane Smith 25" };
            var persons = new List<Person>();

            // Act
            if (int.TryParse(inputLines[0], out var lines))
            {
                for (int i = 1; i <= lines && i < inputLines.Length; i++)
                {
                    var cmdArgs = inputLines[i].Split();
                    if (cmdArgs.Length >= 3 && int.TryParse(cmdArgs[2], out var age))
                    {
                        persons.Add(new Person(cmdArgs[0], cmdArgs[1], age));
                    }
                }
            }

            // Assert
            Assert.Equal(2, persons.Count);
            Assert.Equal("John", persons[0].FirstName);
            Assert.Equal("Jane", persons[1].FirstName);
        }

        [Fact]
        public void ParseInput_WithInvalidAge_SkipsLine()
        {
            // Arrange
            var inputLines = new[] { "3", "John Doe 30", "Jane Smith invalid", "Bob Clark 25" };
            var persons = new List<Person>();

            // Act
            if (int.TryParse(inputLines[0], out var lines))
            {
                for (int i = 1; i <= lines && i < inputLines.Length; i++)
                {
                    var cmdArgs = inputLines[i].Split();
                    if (cmdArgs.Length >= 3 && int.TryParse(cmdArgs[2], out var age))
                    {
                        persons.Add(new Person(cmdArgs[0], cmdArgs[1], age));
                    }
                }
            }

            // Assert
            Assert.Equal(2, persons.Count);
            Assert.Equal("John", persons[0].FirstName);
            Assert.Equal("Bob", persons[1].FirstName);
        }

        [Fact]
        public void ParseInput_WithInsufficientFields_SkipsLine()
        {
            // Arrange
            var inputLines = new[] { "3", "John Doe 30", "Jane Smith", "Bob Clark 25" };
            var persons = new List<Person>();

            // Act
            if (int.TryParse(inputLines[0], out var lines))
            {
                for (int i = 1; i <= lines && i < inputLines.Length; i++)
                {
                    var cmdArgs = inputLines[i].Split();
                    if (cmdArgs.Length >= 3 && int.TryParse(cmdArgs[2], out var age))
                    {
                        persons.Add(new Person(cmdArgs[0], cmdArgs[1], age));
                    }
                }
            }

            // Assert
            Assert.Equal(2, persons.Count);
            Assert.Equal("John", persons[0].FirstName);
            Assert.Equal("Bob", persons[1].FirstName);
        }
    }
}