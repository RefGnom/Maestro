namespace Maestro.TelegramIntegrator.Implementation.Extensions;

public static class EnumerableExtensions
{
    public static string JoinToString<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }
}