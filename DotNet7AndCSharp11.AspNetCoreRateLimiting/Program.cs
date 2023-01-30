using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

const string LimitedRateLimiter = "limited";


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRateLimiter(new RateLimiterOptions()
    .AddTokenBucketLimiter(policyName: LimitedRateLimiter, options =>
    {
        options.TokenLimit = 3;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
        options.TokensPerPeriod = 2;
        options.AutoReplenishment = true;
    }));

app.MapGet("/", () => "I'm limited").RequireRateLimiting(LimitedRateLimiter);
app.MapGet("/admin", () => "Hello Admin!");

app.Run();