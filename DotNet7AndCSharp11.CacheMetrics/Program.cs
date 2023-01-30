using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.AddMemoryCache(o =>
{
    o.TrackStatistics = true;
    o.SizeLimit = 100;
});

var serviceProvider = services.BuildServiceProvider();
var cache = serviceProvider.GetService<IMemoryCache>();




var hello1 = "Hello world!";
cache.Set("1", hello1, new MemoryCacheEntryOptions { Size = hello1.Length });

var hello2 = "Hello underworld!";
cache.Set("3", hello2, new MemoryCacheEntryOptions { Size = hello2.Length });




for (int i = 0; i < 10; i++)
{
    var value = cache.Get<string>($"{i}");
}

var statistics = cache.GetCurrentStatistics();


Console.WriteLine(statistics.CurrentEntryCount);
Console.WriteLine(statistics.CurrentEstimatedSize);
Console.WriteLine(statistics.TotalHits);
Console.WriteLine(statistics.TotalMisses);