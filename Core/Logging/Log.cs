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

public class Log(IDateTimeProvider dateTimeProvider, IWriter writer, string context) : ILog
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IWriter _writer = writer;
    private readonly string _context = context;

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
            $"[{_context}] " +
            $"[{level}] " +
            $"{message}",
            color
        );
    }
}