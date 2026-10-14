using System;
using System.Globalization;

/// <summary>
/// Represents a person with name, age, and salary information.
/// Validates all inputs and enforces business rules for salary calculations.
/// </summary>
public class Person
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _age;
    private decimal _salary;

    public const decimal DefaultMinimumSalary = 460;
    private const int MinimumNameLength = 3;

    /// <summary>Gets or sets the first name. Must be at least 3 characters.</summary>
    public string FirstName
    {
        get => _firstName;
        set
        {
            ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    /// <summary>Gets or sets the last name. Must be at least 3 characters.</summary>
    public string LastName
    {
        get => _lastName;
        set
        {
            ValidateName(value, nameof(LastName));
            _lastName = value;
        }
    }

    /// <summary>Gets or sets the age. Must be greater than 0.</summary>
    public int Age
    {
        get => _age;
        set
        {
            ValidateAge(value);
            _age = value;
        }
    }

    /// <summary>Gets or sets the salary. Must be at least 460 and is rounded to 2 decimal places.</summary>
    public decimal Salary
    {
        get => _salary;
        set
        {
            ValidateSalary(value);
            _salary = Math.Round(value, 2);
        }
    }

    /// <summary>Initializes a new person with name, age, and salary.</summary>
    public Person(string firstName, string lastName, int age, decimal salary = DefaultMinimumSalary)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Salary = salary;
    }

    /// <summary>
    /// Increases the salary by a percentage.
    /// Persons under 30 receive half the percentage increase; those 30+ receive the full increase.
    /// </summary>
    public void IncreaseSalary(decimal percentage)
    {
        if (percentage < 0)
            throw new ArgumentException("Percentage must always be positive!");

        // Age-based salary increase: younger workers receive half the increase as an incentive
        decimal increaseRate = Age < 30 ? percentage / 200 : percentage / 100;
        Salary += Salary * increaseRate;
    }

    /// <summary>Returns a formatted string with the person's name and salary.</summary>
    public override string ToString()
        => $"{FirstName} {LastName} receives {Salary.ToString("F2", CultureInfo.InvariantCulture)} dollars.";

    // Validation helper methods
    private void ValidateName(string? value, string fieldName)
    {
        if (value == null || value.Length < MinimumNameLength)
            throw new ArgumentException($"{fieldName} cannot contain fewer than {MinimumNameLength} symbols!");
    }

    private void ValidateAge(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Age cannot be zero or a negative integer!");
    }

    private void ValidateSalary(decimal value)
    {
        if (value < DefaultMinimumSalary)
            throw new ArgumentException($"Salary cannot be less than {DefaultMinimumSalary} dollar!");
    }
}
