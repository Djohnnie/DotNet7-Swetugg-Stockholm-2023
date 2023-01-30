using System.Threading.RateLimiting;


Console.WriteLine();


RateLimiter limiter = new ConcurrencyLimiter(
    new ConcurrencyLimiterOptions
    {
        PermitLimit = 5,
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        QueueLimit = 2
    });


await Parallel.ForEachAsync(Enumerable.Range(0, 10), async (i, _) =>
{
    await Task.Delay(i * 10);

    using RateLimitLease lease = limiter.AttemptAcquire();
    if (lease.IsAcquired)
    {
        Console.WriteLine($"Rate limited lease acquired for thread {i}");
    }
    else
    {
        Console.WriteLine($"Rate limited lease DENIED for thread {i}");
    }

    await Task.Delay(1000);
});


Console.WriteLine();


await Parallel.ForEachAsync(Enumerable.Range(0, 10), async (i, _) =>
{
    await Task.Delay(i * 10);

    using RateLimitLease lease = await limiter.AcquireAsync();
    if (lease.IsAcquired)
    {
        Console.WriteLine($"Rate limited lease acquired for thread {i}");
    }
    else
    {
        Console.WriteLine($"Rate limited lease DENIED for thread {i}");
    }

    await Task.Delay(1000);
});



Console.ReadKey();