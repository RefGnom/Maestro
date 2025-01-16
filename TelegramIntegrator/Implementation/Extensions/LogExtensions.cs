using System.Diagnostics;
using Maestro.Core.Logging;

namespace Maestro.TelegramIntegrator.Implementation.Extensions;

public static class LogExtensions
{
    public static void WarnWithStackTrace<T>(this ILog<T> log, string message, int traceCount)
    {
        const string separator = "\n";
        var trace = new StackTrace().ToString().Split(separator).Take(traceCount).JoinToString(separator);
        log.Warn($"{message}{separator}{trace}");
    }
}