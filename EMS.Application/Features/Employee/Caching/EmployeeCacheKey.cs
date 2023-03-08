using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace EMS.Application.Features.Employee.Caching;

public static class EmployeeCacheKey
{
    private static readonly TimeSpan refreshInterval = TimeSpan.FromHours(1);
    public const string GetAllCacheKey = "all-Employees";
    public static string GetPaginationCacheKey(string parameters)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(parameters);
        return $"EmployeesWithPaginationQuery-{System.Convert.ToBase64String(plainTextBytes)}";
    }
    static EmployeeCacheKey()
    {
        _tokensource = new CancellationTokenSource(refreshInterval);
    }
    private static CancellationTokenSource _tokensource;
    public static CancellationTokenSource SharedExpiryTokenSource()
    {
        if (_tokensource.IsCancellationRequested)
        {
            _tokensource = new CancellationTokenSource(refreshInterval);
        }
        return _tokensource;
    }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(SharedExpiryTokenSource().Token));
}

