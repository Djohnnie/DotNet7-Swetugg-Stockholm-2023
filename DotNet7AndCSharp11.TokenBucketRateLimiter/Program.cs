using System.Threading.RateLimiting;


Console.WriteLine("Hello, World!");


RateLimiter limiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
{
    TokenLimit = 5,
    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
    QueueLimit = 0,
    ReplenishmentPeriod = TimeSpan.FromSeconds(1),
    TokensPerPeriod = 1,
    AutoReplenishment = true
});

await Parallel.ForEachAsync(Enumerable.Range(0, 10), async (i, _) =>
{
    await Task.Delay(i * 200);

    using RateLimitLease lease = await limiter.AcquireAsync();

    if (lease.IsAcquired)
    {
        Console.WriteLine($"Rate limited lease acquired for thread {i}");
    }
    else
    {
        Console.WriteLine($"Rate limited lease DENIED for thread {i}");
    }
});


Console.ReadKey();