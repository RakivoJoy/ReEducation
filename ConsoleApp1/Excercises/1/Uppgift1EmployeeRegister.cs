using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Uppgift1
{
    public class EmployeeRegister
    {
        private List<Employee> employees = new List<Employee>();

        public void AddEmployee(Employee e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            employees.Add(e);
        }

        public void PrintRegister()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("The register is empty.");
                return;
            }

            foreach (var e in employees)
                Console.WriteLine(e);
        }

        public void SaveToFile(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(employees, options);
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<List<Employee>>(json);

            // Only replace the current list once everything was read OK
            employees = loaded ?? new List<Employee>();
        }


        // Stubs (optional extras)
        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
