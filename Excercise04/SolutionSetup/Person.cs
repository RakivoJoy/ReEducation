public class Person
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _age;
    private decimal _salary;

    public string FirstName
    {
        get { return _firstName; }
        set
        {
            if (value == null || value.Length < 3)
            {
                throw new ArgumentException("First name cannot contain fewer than 3 symbols!");
            }
            _firstName = value;
        }
    }

    public string LastName
    {
        get { return _lastName; }
        set
        {
            if (value == null || value.Length < 3)
            {
                throw new ArgumentException("Last name cannot contain fewer than 3 symbols!");
            }
            _lastName = value;
        }
    }

    public int Age
    {
        get { return _age; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Age cannot be zero or a negative integer!");
            }
            _age = value;
        }
    }

    public decimal Salary
    {
        get { return _salary; }
        set
        {
            if (value < DefaultMinimumSalary)
            {
                throw new ArgumentException($"Salary cannot be less than {DefaultMinimumSalary} dollar!");
            }
            _salary = Math.Round(value, 2);
        }
    }

    public const decimal DefaultMinimumSalary = 460;

    public Person(string firstName, string lastName, int age, decimal salary = Person.DefaultMinimumSalary)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Salary = salary;
    }

    public void IncreaseSalary(decimal percentage)
    {
        if (percentage < 0)
        {
            throw new ArgumentException("Percentage must always be positive!");
        }

        if(Age < 30)
        {
            Salary += Salary * (percentage / 200);
        }
        else
        {
            Salary += Salary * (percentage / 100);
        }
    }

    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName} is {this.Age} years old. Salary: ${this.Salary}";
    }
}
