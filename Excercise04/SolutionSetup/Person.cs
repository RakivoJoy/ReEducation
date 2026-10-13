public class Person
{
    private string _firstName;
    private string _lastName;
    private int _age;

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

    public Person(string firstName, string lastName, int age)
    {
        this._firstName = firstName;
        this._lastName = lastName;
        this._age = age;
    }

    public override string ToString()
    {
        return $"{_firstName} {_lastName}, Age: {_age}";
    }
}
