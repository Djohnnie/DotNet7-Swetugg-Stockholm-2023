using System.Threading.RateLimiting;


Console.WriteLine("Hello, World!");


RateLimiter limiter = new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
{
    PermitLimit = 5,
    SegmentsPerWindow = 5,
    QueueLimit = 0,
    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
    Window = TimeSpan.FromSeconds(1),
    AutoReplenishment = true,
});

await Parallel.ForEachAsync(Enumerable.Range(0, 10), async (i, _) =>
{
    await Task.Delay(i * 500);

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