// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");



class Data<T>
{
    [Custom(Type = typeof(int))]
    [GenericCustom<int>]
    void Method1(int value)
    {

    }

    //[Custom(Type = typeof(T))]
    //[GenericCustom<T>]
    //void Method2()
    //{

    //}
}




class CustomAttribute : Attribute
{
    public Type Type { get; set; }
}

class GenericCustomAttribute<T> : CustomAttribute
{
    public GenericCustomAttribute()
    {
        Type = typeof(T);
    }
}