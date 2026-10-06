using System;
using System.Text.Json.Serialization;

namespace Uppgift1
{
    // Property-based record with validation. A [JsonConstructor] ensures
    // System.Text.Json will call the validating constructor during deserialization.
    public record Employee
    {
        public string Name { get; } = string.Empty;
        public decimal Salary { get; }

        public Employee() { }

        [JsonConstructor]
        public Employee(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name can't be empty", nameof(name));

            if (salary < 0)
                throw new ArgumentOutOfRangeException(nameof(salary), "Salary can't be negative");

            Name = name;
            Salary = salary;
        }

        public override string ToString() => $"{Name} - {Salary} kr";
    }
}
