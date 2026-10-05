using System;
using System.Collections.Generic;

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