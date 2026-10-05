using System;
using System.Xml.Linq;
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


    

   // ---------- Name validation ----------

       [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_EmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Employee(invalidName, 30000m));

            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void Constructor_NullName_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Employee(null, 30000m));

            Assert.Equal("name", ex.ParamName);
        }

        // ---------- Salary validation ----------

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(-32000)]
        public void Constructor_NegativeSalary_ThrowsArgumentOutOfRangeException(int invalidSalary)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Employee("Anna Svensson", invalidSalary));

            Assert.Equal("salary", ex.ParamName);
        }



                // ---------- ToString ----------

        [Fact]
        public void ToString_ReturnsNameAndSalaryFormatted()
        {
            var employee = new Employee("Anna Svensson", 32000m);

            Assert.Equal("Anna Svensson - 32000 kr", employee.ToString());
        }


        // ---------- Immutability ----------
        //*
        //This test checks that Name and Salary can't be assigned to from outside the class.
        //It does this with reflection, which lets code inspect the structure of types at runtime.

        [Fact]
        public void Properties_AreReadOnly()
        {
            var nameProperty = typeof(Employee).GetProperty(nameof(Employee.Name));
            var salaryProperty = typeof(Employee).GetProperty(nameof(Employee.Salary));

            Assert.Null(nameProperty.SetMethod);
            Assert.Null(salaryProperty.SetMethod);
        }
    }

}