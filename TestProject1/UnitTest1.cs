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

    /*
 * TODO: Additional tests for Employee
 *
 * Constructor / validation
 * - Salary just below the boundary (-0.01m) throws ArgumentOutOfRangeException.
 * - Salary just above the boundary (0.01m) is accepted.
 * - Very large salary (decimal.MaxValue) is accepted.
 * - Exception messages: verify the text for the invalid name and invalid salary cases.
 * - Invalid name is checked before invalid salary when both are invalid
 *   (documents which exception is thrown first).
 *
 * Name handling
 * - Name with non-ASCII characters (e.g. "Åsa Öberg") is stored unchanged.
 * - Name with leading/trailing whitespace is stored as-is 
 *
 * ToString
 * - Zero salary produces "Name - 0 kr".
 * - Name containing " - " does not break the output format.
 *
 * Behavior / design
 * - Two Employee objects with identical data are not equal (reference equality),
 *   or are equal if Equals/GetHashCode is implemented later.
 * - Private fields are readonly (reflection: FieldInfo.IsInitOnly is true).
 *
 * Test style
 * - Convert repeated cases to [Theory] with [InlineData] or [MemberData]
 *   (decimal values can't go in [InlineData] as decimal literals, so use
 *   double/int there or [MemberData]).
 */

}