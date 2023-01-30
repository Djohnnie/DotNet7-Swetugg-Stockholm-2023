
using System.Diagnostics.CodeAnalysis;

Console.WriteLine("Hello, World!");


//_ = new Person
//{
//    Name = "Hooyberghs"
//};

_ = new Student("Hooyberghs", "Johnny", "Hogwarts School of Witchcraft and Wizardry");


public class Person
{
    public required string Name { get; init; }
    public required string FirstName { get; init; }
    public int Age { get; init; }
}


public class Student : Person
{
    public required string School { get; init; }

    [SetsRequiredMembers]
    public Student(string name, string firstName, string school)
    {
        Name = name;
        FirstName = firstName;
        School = school;
    }
}