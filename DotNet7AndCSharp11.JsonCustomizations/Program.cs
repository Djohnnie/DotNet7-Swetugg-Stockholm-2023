using System.Text.Json.Serialization.Metadata;
using System.Text.Json;



JsonSerializerOptions options = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    {
        Modifiers = { PersonTypeInfo }
    }
};

var p = new Person("Johnny Hooyberghs");

var serialized = JsonSerializer.Serialize(p, options);
var deserialized = JsonSerializer.Deserialize<Person>(serialized, options);


Console.WriteLine(serialized);
Console.WriteLine(deserialized);


static void PersonTypeInfo(JsonTypeInfo jsonTypeInfo)
{
    if (jsonTypeInfo.Type != typeof(Person))
        return;

    JsonPropertyInfo property = jsonTypeInfo.CreateJsonPropertyInfo(typeof(string), "Name");
    property.Get = (obj) =>
    {
        Person p = (Person)obj;
        return p.GetName();
    };

    property.Set = (obj, val) =>
    {
        Person p = (Person)obj;
        string value = (string)val;
        p.SetName(value);
    };

    jsonTypeInfo.Properties.Add(property);
}


class Person
{
    private string _name = string.Empty;

    public string GetName() => _name;

    public void SetName(string name)
    {
        _name = name;
    }

    public Person(string name)
    {
        SetName(name);
    }

    public override string ToString()
    {
        return $"{_name}";
    }
}