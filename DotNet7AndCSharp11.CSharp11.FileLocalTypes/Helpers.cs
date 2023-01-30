namespace DotNet7AndCSharp11.CSharp11.FileLocalTypes;

internal class Helpers
{
    public string GetSomething()
    {
        return $"{new Whatever()}";
    }
}

file class Whatever
{
    public override string ToString()
    {
        return "Hello File Local Type!";
    }
}