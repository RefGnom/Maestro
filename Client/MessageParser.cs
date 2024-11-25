using Core;
using Core.DateTime;
using Data.Models;

namespace Client;

public class MessagesParser : IMessageParser<Event>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public MessagesParser(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public Result<Event> ParseMessage(string message)
    {
        if (message.StartsWith("/create"))
        {
            var parts = message.Split(" ", 4);
            if (parts.Length == 3)
            {
                if (_dateTimeProvider.TryParseDateTime(parts[1], parts[2], out var dateTime))
                {
                    return Result.CreateSuccess(new Event() { ReminderTime = dateTime, Description = parts[2] });
                }
            }

            if (parts.Length == 4)
            {
                if (_dateTimeProvider.TryParseDateTime(parts[1], parts[2], out var dateTime))
                {
                    return Result.CreateSuccess(new Event() { ReminderTime = dateTime, Description = parts[3] });
                }
            }
        }

        return Result.CreateFailure<Event>("Incorrect message entered");
    }
}