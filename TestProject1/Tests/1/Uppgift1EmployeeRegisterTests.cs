using System;
using System.IO;
using Xunit;

namespace Uppgift1.Tests
{
    public class EmployeeRegisterTests
    {
        // Runs PrintRegister and returns what it wrote to the console.
        private static string CapturePrintOutput(EmployeeRegister register)
        {
            var originalOut = Console.Out;
            try
            {
                var writer = new StringWriter();
                Console.SetOut(writer);
                register.PrintRegister();
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void AddEmployee_Null_Throws()
        {
            var register = new EmployeeRegister();

            Assert.Throws<ArgumentNullException>(() => register.AddEmployee(null));
        }

        [Fact]
        public void PrintRegister_Empty_PrintsEmptyMessage()
        {
            var register = new EmployeeRegister();

            string output = CapturePrintOutput(register);

            Assert.Equal("The register is empty." + Environment.NewLine, output);
        }

        [Fact]
        public void PrintRegister_WithOneEmployee_PrintsEmployee()
        {
            var register = new EmployeeRegister();
            register.AddEmployee(new Employee("Anna Svensson", 32000m));

            string output = CapturePrintOutput(register);

            Assert.Equal("Anna Svensson - 32000 kr" + Environment.NewLine, output);
        }

        [Fact]
        public void PrintRegister_WithTwoEmployees_PrintsInOrderAdded()
        {
            var register = new EmployeeRegister();
            register.AddEmployee(new Employee("Anna Svensson", 32000m));
            register.AddEmployee(new Employee("Erik Berg", 28000m));

            string output = CapturePrintOutput(register);

            string expected =
                "Anna Svensson - 32000 kr" + Environment.NewLine +
                "Erik Berg - 28000 kr" + Environment.NewLine;
            Assert.Equal(expected, output);
        }

        // TODO: GetCount and IsEmpty tests, once those methods are implemented.
    }
}
