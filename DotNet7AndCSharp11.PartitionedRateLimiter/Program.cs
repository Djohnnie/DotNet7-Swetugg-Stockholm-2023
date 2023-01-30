
using System.Threading.RateLimiting;

Console.WriteLine("Hello, World!");

PartitionedRateLimiter<MyPolicyEnum> limiter = PartitionedRateLimiter.Create<MyPolicyEnum, MyPolicyEnum>(resource =>
{
    switch (resource)
    {
        case MyPolicyEnum.Odd:
            return RateLimitPartition.GetFixedWindowLimiter(MyPolicyEnum.Odd, x => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 4,
                Window = TimeSpan.FromSeconds(1)
            });
        case MyPolicyEnum.Even:
            return RateLimitPartition.GetTokenBucketLimiter(MyPolicyEnum.Even, x => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 4,
                ReplenishmentPeriod = TimeSpan.MaxValue,
                TokensPerPeriod = 4
            });
        default:
            return RateLimitPartition.GetNoLimiter(MyPolicyEnum.Whatever);
    }
});


await Parallel.ForEachAsync(Enumerable.Range(0, 10), async (i, _) =>
{
    using RateLimitLease lease = await limiter.AcquireAsync(i % 2 == 0 ? MyPolicyEnum.Even : MyPolicyEnum.Odd);

    if (lease.IsAcquired)
    {
        Console.WriteLine($"Rate limited lease acquired for thread {i} [{lease.GetType()}]");
    }
    else
    {
        Console.WriteLine($"Rate limited lease DENIED for thread {i} [{lease.GetType()}]");
    }
});





Console.ReadKey();


enum MyPolicyEnum
{
    Even,
    Odd,
    Whatever
}