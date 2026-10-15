using SolutionSetup.Models;

namespace SolutionSetup.Generators
{
    /// <summary>
    /// Produces fake HR employee data for benchmarks and tests.
    /// </summary>
    public static class PersonGenerator
    {
        private static readonly string[] FirstNames = ["Anna", "Ben", "Chloe", "David", "Emma", "Felix", "Grace", "Hugo"];
        private static readonly string[] LastNames = ["Smith", "Brown", "Lee", "Garcia", "Muller", "Rossi", "Kim", "Novak"];
        private static readonly string[] Departments = ["HR", "Finance", "IT", "Sales", "Marketing", "Operations"];

        /// <summary>
        /// Generates <paramref name="count"/> persons with sequential unique Ids starting at 1.
        /// A seeded <see cref="Random"/> makes output reproducible, so benchmarks and tests see identical data.
        /// </summary>
        /// <param name="count">Number of persons to create.</param>
        /// <param name="seed">Optional seed for deterministic output.</param>
        public static List<Person> Generate(int count, int? seed = null)
        {
            var random = seed.HasValue ? new Random(seed.Value) : new Random();
            var people = new List<Person>(count);

            for (int i = 1; i <= count; i++)
            {
                var first = FirstNames[random.Next(FirstNames.Length)];
                var last = LastNames[random.Next(LastNames.Length)];
                var hireDate = DateTime.Today.AddDays(-random.Next(1, 365 * 15));

                people.Add(new Person
                {
                    Id = i,
                    Name = $"{first} {last}",
                    DateOfBirth = DateTime.Today.AddYears(-random.Next(22, 65)),
                    Department = Departments[random.Next(Departments.Length)],
                    Salary = random.Next(30_000, 150_001),
                    HireDate = hireDate,
                    Email = $"{first}.{last}{i}@company.com".ToLowerInvariant()
                });
            }

            return people;
        }
    }
}
