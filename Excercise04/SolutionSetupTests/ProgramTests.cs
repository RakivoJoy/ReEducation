namespace SolutionSetupTests
{
    public class ProgramTests
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
        }
    }
}
