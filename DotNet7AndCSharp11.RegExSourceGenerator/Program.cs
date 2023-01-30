using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.RegularExpressions;

BenchmarkRunner.Run<RegExBenchmarks>();


public partial class RegExBenchmarks
{
    [GeneratedRegex(@"abc|def", RegexOptions.IgnoreCase)] 
    public static partial Regex MyRegex();

    public static Regex _regex = new Regex(@"abc|def");

    [Benchmark(Description = "RegEx Code Generator")]
    public bool RegexCodeGenerator()
    {
        return MyRegex().IsMatch("abcdefghijklmnopqrstuvwxyz");
    }

    [Benchmark(Description = "Static RegEx")]
    public bool StaticRegex()
    {
        return _regex.IsMatch("abcdefghijklmnopqrstuvwxyz");
    }
}