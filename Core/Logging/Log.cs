using System.Globalization;
using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class Log<TContext>(IDateTimeProvider dateTimeProvider, IWriter writer) : ILog<TContext>
{
    public void Info(string message)
    {
        LogEvent("Info", message, ConsoleColor.Gray);
    }

    public void Warn(string message)
    {
        LogEvent("Warn", message, ConsoleColor.DarkYellow);
    }

    public void Error(string message)
    {
        LogEvent("Error", message, ConsoleColor.DarkRed);
    }

    private void LogEvent(string level, string message, ConsoleColor color)
    {
        writer.WriteLine(
            $"[{dateTimeProvider.GetCurrentDateTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)}] " +
            $"[{typeof(TContext)}] " +
            $"[{level}] " +
            $"{message}",
            color
        );
    }
}