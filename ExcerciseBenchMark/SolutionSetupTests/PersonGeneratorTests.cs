using SolutionSetup.Generators;

namespace SolutionSetupTests
{
    /// <summary>
    /// Verifies PersonGenerator output: count, unique Ids, determinism and field validity.
    /// </summary>
    public class PersonGeneratorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void Generate_ReturnsRequestedCount(int count)
        {
            Assert.Equal(count, PersonGenerator.Generate(count, seed: 1).Count);
        }

        [Fact]
        public void Generate_IdsAreUnique()
        {
            var people = PersonGenerator.Generate(1000, seed: 1);

            Assert.Equal(people.Count, people.Select(p => p.Id).Distinct().Count());
        }

        [Fact]
        public void Generate_SameSeed_ProducesIdenticalData()
        {
            var first = PersonGenerator.Generate(50, seed: 42);
            var second = PersonGenerator.Generate(50, seed: 42);

            Assert.Equal(first.Select(p => (p.Name, p.Salary, p.Department)),
                         second.Select(p => (p.Name, p.Salary, p.Department)));
        }

        [Fact]
        public void Generate_DifferentSeeds_ProduceDifferentData()
        {
            var first = PersonGenerator.Generate(50, seed: 1);
            var second = PersonGenerator.Generate(50, seed: 2);

            Assert.NotEqual(first.Select(p => p.Salary), second.Select(p => p.Salary));
        }

        [Fact]
        public void Generate_FieldsAreValid()
        {
            var people = PersonGenerator.Generate(200, seed: 7);

            Assert.All(people, p =>
            {
                Assert.False(string.IsNullOrWhiteSpace(p.Name));
                Assert.Contains("@", p.Email);
                Assert.False(string.IsNullOrWhiteSpace(p.Department));
                Assert.True(p.Salary > 0);
                Assert.True(p.HireDate <= DateTime.Today);
                Assert.True(p.DateOfBirth < p.HireDate);
            });
        }
    }
}
