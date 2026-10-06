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

        [Fact]
        public void Properties_AreReadOnly()
        {
            var nameProperty = typeof(Employee).GetProperty(nameof(Employee.Name));
            var salaryProperty = typeof(Employee).GetProperty(nameof(Employee.Salary));

            // Accept either no setter (classic read-only) or an init-only setter
            // (C# 9+ record init). Detect init-only by checking for the
            // IsExternalInit required custom modifier on the setter parameter.
            bool HasInitOnlySetter(System.Reflection.PropertyInfo p)
            {
                var set = p.SetMethod;
                if (set == null) return false;

                var parameters = set.GetParameters();
                if (parameters.Length == 0) return false;

                var mods = parameters[0].GetRequiredCustomModifiers();
                foreach (var m in mods)
                {
                    if (m.FullName == "System.Runtime.CompilerServices.IsExternalInit")
                        return true;
                }
                return false;
            }

            Assert.True(nameProperty.SetMethod == null || HasInitOnlySetter(nameProperty));
            Assert.True(salaryProperty.SetMethod == null || HasInitOnlySetter(salaryProperty));
        }

        [Fact]
        public void Constructor_SalaryJustBelowBoundary_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Employee("Anna", -0.01m));

            Assert.Equal("salary", ex.ParamName);
        }

        [Fact]
        public void Constructor_SalaryJustAboveBoundary_IsAccepted()
        {
            var employee = new Employee("Junior", 0.01m);

            Assert.Equal(0.01m, employee.Salary);
        }

        [Fact]
        public void Constructor_VeryLargeSalary_IsAccepted()
        {
            var employee = new Employee("Ceo", decimal.MaxValue);

            Assert.Equal(decimal.MaxValue, employee.Salary);
        }

        [Fact]
        public void Constructor_InvalidNameCheckedBeforeSalary()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Employee("", -1m));

            // Name validation should run first
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void Constructor_InvalidName_MessageContainsText()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Employee("", 100m));

            Assert.Contains("Name can't be empty", ex.Message);
        }

        [Fact]
        public void Constructor_NegativeSalary_MessageContainsText()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Employee("Anna", -5m));

            Assert.Contains("Salary can't be negative", ex.Message);
        }

    }

}