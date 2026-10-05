using System;
using System.Collections.Generic;

namespace Uppgift1
{
    /// <summary>
    /// Holds the collection of employees, adds new ones and prints the register.
    /// </summary>
    public class EmployeeRegister
    {
        private readonly List<Employee> employees;

        public EmployeeRegister()
        {
            employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employees.Add(employee);
        }

        public void PrintRegister()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("The register is empty.");
                return;
            }

            foreach (Employee employee in employees)
            {
                Console.WriteLine(employee);
            }
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