namespace SolutionSetup.Models
{
    /// <summary>
    /// HR employee record. Equality is based on <see cref="Id"/> only, so it is safe to use as a HashSet/Dictionary key.
    /// </summary>
    public class Person
    {
        /// <summary>Unique employee identifier.</summary>
        public int Id { get; set; }

        /// <summary>Full name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Date of birth.</summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>Department name.</summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>Annual salary.</summary>
        public decimal Salary { get; set; }

        /// <summary>Date the employee was hired.</summary>
        public DateTime HireDate { get; set; }

        /// <summary>Work email address.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Two persons are equal when their Id matches.</summary>
        public override bool Equals(object? obj) => obj is Person other && other.Id == Id;

        /// <summary>Hash code derived from Id, consistent with <see cref="Equals(object?)"/>.</summary>
        public override int GetHashCode() => Id;
    }
}
