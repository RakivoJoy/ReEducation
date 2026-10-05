using System;
using System.IO;
using Xunit;

namespace Uppgift1.Tests
{
    // Console redirection is global, so test classes that use it must not run in parallel.
    [Collection("Console")]
    public class ProgramTests
    {
        // Feeds the lines to the program as console input and returns everything it printed.
        private static string RunProgram(params string[] inputLines)
        {
            var originalIn = Console.In;
            var originalOut = Console.Out;
            try
            {
                var writer = new StringWriter();
                Console.SetIn(new StringReader(string.Join(Environment.NewLine, inputLines)));
                Console.SetOut(writer);
                Program.Main(new string[0]);
                return writer.ToString();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void Main_ExitChoice_ShowsMenuAndEnds()
        {
            string output = RunProgram("0");

            Assert.Contains("1. Add employee", output);
            Assert.Contains("2. Print register", output);
            Assert.Contains("0. Exit", output);
        }

        [Fact]
        public void Main_InvalidChoice_PrintsErrorMessage()
        {
            string output = RunProgram("9", "0");

            Assert.Contains("Invalid choice.", output);
        }

        [Fact]
        public void Main_AddEmployeeThenPrint_ShowsEmployee()
        {
            string output = RunProgram("1", "Anna Svensson", "32000", "2", "0");

            Assert.Contains("Employee added.", output);
            Assert.Contains("Anna Svensson - 32000 kr", output);
        }

        [Fact]
        public void Main_PrintWithoutEmployees_ShowsEmptyMessage()
        {
            string output = RunProgram("2", "0");

            Assert.Contains("The register is empty.", output);
        }

        // ---------- TODO: Stubs ----------

        [Fact(Skip = "TODO")]
        public void Main_NonNumericSalary_EmployeeIsNotAdded()
        {
        }

        [Fact(Skip = "TODO")]
        public void Main_NegativeSalary_EmployeeIsNotAdded()
        {
        }

        [Fact(Skip = "TODO")]
        public void Main_EmptyName_EmployeeIsNotAdded()
        {
        }

        [Fact(Skip = "TODO")]
        public void Main_EndOfInput_ExitsWithoutError()
        {
        }
    }
}
