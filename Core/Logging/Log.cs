using System.Globalization;
using Core.IO;
using Core.Providers;

namespace Core.Logging;

public class Log(IDateTimeProvider dateTimeProvider, IWriter writer, string context = "Program.Main") : ILog
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
        _writer.WriteLine($"[{_dateTimeProvider.GetCurrentDateTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)}] " +
                          $"[{level}] " +
                          $"{message}", color);
    }
}