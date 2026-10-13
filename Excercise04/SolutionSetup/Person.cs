public class Person
{
    private string _firstName;
    private string _lastName;
    private int _age;
    private decimal _salary;

    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }

    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; }
    }

    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }

    public decimal Salary
    {
        get { return _salary; }
        set { _salary = value; }
    }

    public const decimal DefaultMinimumSalary = 460;

    public Person(string firstName, string lastName, int age, decimal salary = Person.DefaultMinimumSalary)
    {
        if (firstName == null || firstName.Length < 3)
        {
            throw new ArgumentException("First name cannot contain fewer than 3 symbols!");
        }

        if (lastName == null || lastName.Length < 3)
        {
            throw new ArgumentException("Last name cannot contain fewer than 3 symbols!");
        }

        if (age <= 0)
        {
            throw new ArgumentException("Age cannot be zero or a negative integer!");
        }

        if (salary < DefaultMinimumSalary)
        {
            throw new ArgumentException($"Salary cannot be less than {DefaultMinimumSalary} dollar!");
        }

        this._firstName = firstName;
        this._lastName = lastName;
        this._age = age;
        this._salary = salary;
    }

    public void IncreaseSalary(decimal percentage)
    {
        this._salary += this._salary * (percentage / 100);
    }

    public override string ToString()
    {
        return $"{_firstName} {_lastName}, Age: {_age}, Salary: {_salary:F2}";
    }
}
