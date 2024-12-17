using System.Globalization;
using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class Log<TContext>(IDateTimeProvider dateTimeProvider, IWriter writer) : ILog<TContext>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IWriter _writer = writer;

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
        _writer.WriteLine(
            $"[{_dateTimeProvider.GetCurrentDateTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)}] " +
            $"[{typeof(TContext).Name}] " +
            $"[{level}] " +
            $"{message}",
            color
        );
    }
}