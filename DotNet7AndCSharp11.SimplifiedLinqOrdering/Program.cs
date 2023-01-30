
Console.WriteLine("Hello, World!");



var data = Enumerable.Range(0, 10);

var sortedDescendingOld = data.OrderByDescending(x => x);
var sortedDescendingNew = data.OrderDescending();

Console.WriteLine(string.Join(',', sortedDescendingOld.Select(x => $"{x}")));
Console.WriteLine(string.Join(',', sortedDescendingNew.Select(x => $"{x}")));