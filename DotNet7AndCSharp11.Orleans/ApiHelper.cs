using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace DotNet7AndCSharp11.Orleans;

public interface IApiHelper<TLogic>
{
    Task<IResult> Execute<TResult>(Expression<Func<TLogic, Task<TResult>>> logicCall);
    Task<IResult> Execute(Func<TLogic, Task> logicCall);
    Task<IResult> Post<TResult>(Func<TLogic, Task<TResult>> logicCall);
}

public class ApiHelper<TLogic> : IApiHelper<TLogic>
{
    private readonly TLogic _logic;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiHelper<TLogic>> _logger;

    public ApiHelper(TLogic logic, IMemoryCache memoryCache, ILogger<ApiHelper<TLogic>> logger)
    {
        _logic = logic;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<IResult> Execute<TResult>(Expression<Func<TLogic, Task<TResult>>> logicCallExpression)
    {
        return await Try(async () =>
        {
            var logicCall = logicCallExpression.Compile();

            if (_memoryCache != null)
            {
                var logicCallName = $"{logicCallExpression.Body}";

                if (!_memoryCache.TryGetValue(logicCallName, out TResult result))
                {
                    result = await logicCall(_logic);
                    _memoryCache.Set(logicCallName, result, TimeSpan.FromSeconds(1));
                }

                return result != null ? Results.Ok(result) : Results.NotFound();
            }
            else
            {
                TResult result = await logicCall(_logic);
                return result != null ? Results.Ok(result) : Results.NotFound();
            }
        });
    }

    public async Task<IResult> Execute(Func<TLogic, Task> logicCall)
    {
        return await Try(async () =>
        {
            await logicCall(_logic);
            return Results.Ok();
        });
    }

    public async Task<IResult> Post<TResult>(Func<TLogic, Task<TResult>> logicCall)
    {
        return await Try(async () =>
        {
            var result = await logicCall(_logic);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
    }

    private async Task<IResult> Try(Func<Task<IResult>> action)
    {
        try
        {
            //using var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var result = await action();

            _logger.LogTrace($"REQUEST: {stopwatch.ElapsedMilliseconds}ms");

            return result;
        }
        catch (/*Logic*/Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}