using System.Globalization;
using Core;
using Data.Models;

namespace Client;
public static class MessagesParser
{
    private static readonly string DateTimeFormat = "yyyy.MM.dd HH:mm";
    
    public static Result<Event> ParseMessage(string message)
    {
        if (message.StartsWith("/create"))
        {
            var parts = message.Split(" ", 4);
            if (parts.Length == 4)
            {
                var date = parts[1];
                var time = parts[2];
                var description = parts[3];
                if (DateTime.TryParseExact($"{date} {time}", DateTimeFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    return Result<Event>.CreateSuccess(new Event() { ReminderTime = dateTime, Description = description});
                }
            }
        }
        return Result<Event>.CreateFailure("Incorrect message entered");
    }
}