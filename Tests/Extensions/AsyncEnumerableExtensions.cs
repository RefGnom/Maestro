namespace Maestro.Tests.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items)
    {
        var results = new List<T>();
        
        await foreach (var item in items.ConfigureAwait(false))
            results.Add(item);
        
        return results;
    }
}