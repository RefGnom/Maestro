using System.Globalization;
using Core.IO;
using Core.Providers;

namespace Core.Logging;

public class Log(IDateTimeProvider dateTimeProvider, IWriter writer, string context = "Program.Main") : ILog
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
            $"[{context}] " +
            $"[{level}] " +
            $"{message}",
            color
        );
    }
}