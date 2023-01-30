

Console.WriteLine(new Data());
Console.WriteLine(default(Data));



readonly struct Data
{
    public decimal Number { get; init; }
    public string Text { get; init; }
    public DateOnly Date { get; init; }

    public Data()
    {
        Number = decimal.MinValue;
        Text = "none";
    }

    public override string ToString() => $"Number: {Number}, Text: {Text}, Date: {Date}";
}