

Method(3);





[Test(ParameterName = nameof(value))]
void Method(int value)
{
    Console.WriteLine(value);
}




[AttributeUsage(AttributeTargets.Method)]
class TestAttribute : Attribute
{
    public string ParameterName { get; set; }
}