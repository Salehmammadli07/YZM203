var employees = new Employee[]
{
    new Employee() { FirstName = "Elvan", LastName = "Negis", Salary = 75_000m},
    new Employee() { FirstName = "Fatih", LastName = "Gulsever", Salary = 150_000m},
    new Employee() { FirstName = "Ahmet", LastName = "Yılmaz", Salary = 100_000m},
    new Employee() { FirstName = "Mehtap", LastName = "Ay", Salary = 75_000m},
    new Employee() { FirstName = "Arda", LastName = "Güler", Salary = 3_000_000m}
};

for (int i = 0; i < employees.Length; i++)
{
    System.Console.WriteLine($"{employees[i].FirstName}, {employees[i].LastName}, {employees[i].Salary}");
}

static void NumberAppSample()
{
    // Bir sınıfı nasıl kullanacağız?
    int x = 5; // readonly-struct // value Type

    // class -> Ref Type
    Number number = new Number(); // Yapıcı metot burada çalışır
                                  // Yapıcı metot ile birlikte mantıksal varlık, fiziksel varlığına dönüşür.
                                  // Referans verilir. 

    // Yapıcı metot ile birlikte mantıksal varlık, fiziksel varlığına dönüşür.
    // Referans verilir.
    System.Console.WriteLine($"Max = {number.GetMax()}");
    System.Console.WriteLine($"Min = {number.GetMin()}");
}

static void Employee()
{
    Employee emp1 = new Employee();
    emp1.FirstName = "Mehtap";
    emp1.LastName = "Ay";
    emp1.Salary = 75_000m;

    System.Console.WriteLine($"Adınız: {emp1.FirstName}");
    System.Console.WriteLine($"Soyadınız: {emp1.LastName}");
    System.Console.WriteLine($"Maaşınız: {emp1.Salary}");

    Employee emp2 = new Employee()
    {
        FirstName = "Arda",
        LastName = "Güler",
        Salary = 3_000_000m

    };
}