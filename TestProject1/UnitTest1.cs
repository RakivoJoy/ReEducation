using System;
using Xunit;

namespace Uppgift1.Tests
{
    public class EmployeeTests
    {
        // ---------- Constructor / properties ----------

        [Fact]
        public void Constructor_ValidInput_SetsName()
        {
            var employee = new Employee("Anna Svensson", 32000m);

            Assert.Equal("Anna Svensson", employee.Name);
        }

        [Fact]
        public void Constructor_ValidInput_SetsSalary()
        {
            var employee = new Employee("Anna Svensson", 32000m);

            Assert.Equal(32000m, employee.Salary);
        }

        [Fact]
        public void Constructor_ZeroSalary_IsAllowed()
        {
            var employee = new Employee("Intern", 0m);

            Assert.Equal(0m, employee.Salary);
        }


    }

}