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
        }

        [Fact]
        public void ToString_WithValidPerson_ReturnsFormattedString()
        {
            // Arrange
            var person = new Person("Jane", "Smith", 25);

            // Act
            var result = person.ToString();

            // Assert
        }

        [Fact]
        public void ToString_WithDifferentAges_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person("Bob", "Johnson", 45);

            // Act
            var result = person.ToString();

            // Assert
        }

        [Fact]
        public void Constructor_WithDifferentNames_InitializesPerson()
        {
            // Arrange & Act
            var person = new Person("Alice", "Williams", 28);

            // Assert
        }
    }
}
