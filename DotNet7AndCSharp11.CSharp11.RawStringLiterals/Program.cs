using System.Text.Json;
using static System.Console;



int x = 2;
int y = 3;
int w = 5;
int h = 1;



var json = $$"""
    {
        "Rectangles": [
            {
                "X": {{x}},
                "Y": {{y}},
                "Width": {{w}},
                "Height": {{h}}
            },
            {
                "X": {{x}},
                "Y": {{y}},
                "Width": {{w}},
                "Height": {{h}}
            }
        ]
    }
    """;


_ = JsonSerializer.Deserialize<Root>(json);

WriteLine(json);


public class Root
{
    public List<Rectangle> Rectangles { get; set; }
}

public class Rectangle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}