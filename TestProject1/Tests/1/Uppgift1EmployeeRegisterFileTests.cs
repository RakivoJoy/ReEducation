using System;
using System.IO;
using System.Text.Json;
using Xunit;

namespace Uppgift1.Tests
{
    // Uses the console (via PrintRegister), so it shares the "Console" collection.
    [Collection("Console")]
    public class EmployeeRegisterFileTests : IDisposable
    {
        // A fresh temp file for every test (xUnit creates a new instance per test)
        private readonly string tempPath =
            Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        public void Dispose()
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }

        // Files in the TestData folder, copied next to the test dll (see csproj)
        private static string TestDataPath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "Tests", "1", "TestData", fileName);
        }

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

        // ---------- Save ----------

        [Fact]
        public void SaveToFile_CreatesFileWithEmployees()
        {
            var register = new EmployeeRegister();
            register.AddEmployee(new Employee("Anna Svensson", 32000m));

            register.SaveToFile(tempPath);

            Assert.True(File.Exists(tempPath));
            Assert.Contains("Anna Svensson", File.ReadAllText(tempPath));
        }

        [Fact]
        public void SaveToFile_EmptyRegister_WritesEmptyList()
        {
            new EmployeeRegister().SaveToFile(tempPath);

            Assert.Equal("[]", File.ReadAllText(tempPath).Trim());
        }

        [Fact]
        public void SaveThenLoad_GivesSameEmployees()
        {
            var original = new EmployeeRegister();
            original.AddEmployee(new Employee("Anna Svensson", 32000m));
            original.AddEmployee(new Employee("Erik Berg", 28000m));
            original.SaveToFile(tempPath);

            var loaded = new EmployeeRegister();
            loaded.LoadFromFile(tempPath);

            Assert.Equal(CapturePrintOutput(original), CapturePrintOutput(loaded));
        }

        // ---------- Load ----------

        [Fact]
        public void LoadFromFile_ValidFile_LoadsEmployees()
        {
            var register = new EmployeeRegister();

            register.LoadFromFile(TestDataPath("Uppgift1ValidRegister.json"));

            string expected =
                "Anna Svensson - 32000 kr" + Environment.NewLine +
                "Erik Berg - 28000 kr" + Environment.NewLine;
            Assert.Equal(expected, CapturePrintOutput(register));
        }

        [Fact]
        public void LoadFromFile_EmptyList_GivesEmptyRegister()
        {
            var register = new EmployeeRegister();

            register.LoadFromFile(TestDataPath("Uppgift1EmptyRegister.json"));

            Assert.Equal("The register is empty." + Environment.NewLine, CapturePrintOutput(register));
        }

        [Fact]
        public void LoadFromFile_ReplacesExistingEmployees()
        {
            var register = new EmployeeRegister();
            register.AddEmployee(new Employee("Old Employee", 10000m));

            register.LoadFromFile(TestDataPath("Uppgift1ValidRegister.json"));

            Assert.DoesNotContain("Old Employee", CapturePrintOutput(register));
        }

        // ---------- Errors ----------

        [Fact]
        public void LoadFromFile_MissingFile_Throws()
        {
            var register = new EmployeeRegister();

            Assert.Throws<FileNotFoundException>(() => register.LoadFromFile(tempPath));
        }

        [Fact]
        public void LoadFromFile_InvalidJson_Throws()
        {
            var register = new EmployeeRegister();

            Assert.Throws<JsonException>(
                () => register.LoadFromFile(TestDataPath("Uppgift1InvalidJson.json")));
        }

        [Fact]
        public void LoadFromFile_NegativeSalary_Throws()
        {
            var register = new EmployeeRegister();

            // Employee rejects the salary; the exact exception type depends
            Assert.ThrowsAny<Exception>(
                () => register.LoadFromFile(TestDataPath("Uppgift1NegativeSalary.json")));
        }

        [Fact]
        public void LoadFromFile_Fails_KeepsExistingEmployees()
        {
            var register = new EmployeeRegister();
            register.AddEmployee(new Employee("Anna Svensson", 32000m));

            try
            {
                register.LoadFromFile(TestDataPath("Uppgift1InvalidJson.json"));
            }
            catch (JsonException)
            {
            }

            Assert.Contains("Anna Svensson", CapturePrintOutput(register));
        }
    }
}
