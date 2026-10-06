class Employee
{
    private string _firstName;   // field (alan)

    private String _lastName;   // field (alan)

    private Decimal _salary;   // field (alan)
    public String LastName
    {
        get { return _lastName.ToUpper(); }
        set { _lastName = value; }
    }
    public String FirstName
    {
        get // accessor
        {
            return _firstName.ToUpper();
        }
        set // accessor
        {
            _firstName = value;
        }
    }

    public Decimal Salary
    {
        get
        {
            return _salary;
        }
        set
        {
            if (value < 28_000m) _salary = 28_000m;
            else _salary = value;
        }
    }
    public Employee()
    {
        System.Console.WriteLine("Employee ctor çalıştı");
    }

}