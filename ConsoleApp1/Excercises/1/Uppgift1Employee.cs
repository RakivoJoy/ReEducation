using System;

namespace Uppgift1
{
    public class Employee
    {
        private string name;
        private decimal salary;

        public string Name => name;
        public decimal Salary => salary;

        public Employee(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name can't be empty", "name");

            if (salary < 0)
                throw new ArgumentOutOfRangeException("salary", "Salary can't be negative");

            this.name = name;
            this.salary = salary;
        }

        public override string ToString()
        {
            return name + " - " + salary + " kr";
        }
    }
}