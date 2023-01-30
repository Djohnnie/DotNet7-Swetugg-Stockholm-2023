using System.ComponentModel;


DoSomething("Hello, World!");


[Description(nameof(message))]
static void DoSomething(string message)
{
    Console.WriteLine(message);
}