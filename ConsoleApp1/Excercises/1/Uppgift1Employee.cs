using System;

namespace Uppgift1
{
    /// <summary>
    /// Represents a single employee with a name and a salary.
    /// </summary>
    public class Employee
    {
        private readonly string name;
        private readonly decimal salary;

        public string Name
        {
            get { return name; }
        }

        public decimal Salary
        {
            get { return salary; }
        }

        public Employee(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            }

            if (salary < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(salary), "Salary cannot be negative.");
            }

            this.name = name;
            this.salary = salary;
        }

        public override string ToString()
        {
            return $"{name} - {salary} kr";
        }
    }
}