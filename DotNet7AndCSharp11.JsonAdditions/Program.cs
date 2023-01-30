using System.Net.Http.Json;
using System.Text.Json;

var subNode = new Node();

var node = new Node
{
    Left = new Node
    {
        Left = subNode,
        Right = subNode
    },
    Right = new Node
    {
        Left = subNode,
        Right = subNode
    }
};

subNode.Left = node;
subNode.Right = node;

var serialized = JsonSerializer.Serialize(node, new JsonSerializerOptions { MaxDepth = 4 });
Console.WriteLine(serialized);

var client = new HttpClient();
client.PatchAsJsonAsync("api/data", new PatchData { Name = "Something" });


class PatchData
{
    public string Name { get; set; }
}


class Node
{
    public Node Left { get; set; }
    public Node Right { get; set; }
}