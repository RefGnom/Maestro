namespace Maestro.Core.Extensions;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var element in enumerable) action(element);
    }

    public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
    {
        foreach (var element in enumerable) await action(element);
    }
}