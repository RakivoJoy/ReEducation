using SolutionSetup.Models;

namespace SolutionSetupTests
{
    /// <summary>
    /// Verifies Person equality contract and property behavior.
    /// </summary>
    public class PersonTests
    {
        [Fact]
        public void Equals_SameId_ReturnsTrue()
        {
            var a = new Person { Id = 1, Name = "A" };
            var b = new Person { Id = 1, Name = "B" };

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentId_ReturnsFalse()
        {
            var a = new Person { Id = 1 };
            var b = new Person { Id = 2 };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_NonPersonObject_ReturnsFalse()
        {
            var a = new Person { Id = 1 };

            Assert.False(a.Equals("1"));
        }

        [Fact]
        public void GetHashCode_SameId_ReturnsSameHash()
        {
            var a = new Person { Id = 5, Name = "A" };
            var b = new Person { Id = 5, Name = "B" };

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void HashSet_DuplicateId_IsNotAddedTwice()
        {
            var set = new HashSet<Person> { new() { Id = 7 }, new() { Id = 7 } };

            Assert.Single(set);
        }

        [Fact]
        public void Properties_RetainAssignedValues()
        {
            var hired = new DateTime(2020, 1, 15);
            var person = new Person
            {
                Id = 3,
                Name = "Jane Doe",
                DateOfBirth = new DateTime(1990, 5, 1),
                Department = "IT",
                Salary = 85_000m,
                HireDate = hired,
                Email = "jane@company.com"
            };

            Assert.Equal(3, person.Id);
            Assert.Equal("Jane Doe", person.Name);
            Assert.Equal("IT", person.Department);
            Assert.Equal(85_000m, person.Salary);
            Assert.Equal(hired, person.HireDate);
            Assert.Equal("jane@company.com", person.Email);
        }
    }
}
