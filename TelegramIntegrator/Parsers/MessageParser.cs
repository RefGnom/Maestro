using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public class MessagesParser(IDateTimeParser dateTimeParser) : IMessageParser
{
    private readonly IDateTimeParser _dateTimeProvider = dateTimeParser;

    public bool TryParse(string message, out Message? messageResult)
    {
        if (!message.StartsWith("/create"))
        {
            messageResult = null;
            return false;
        }

        var parts = message.Split(" ");
        var command = parts[0];
        var date = parts[1];
        var time = parts[2];
        var hasDate = time.All(c => !char.IsLetter(c));
        if (hasDate)
        {
            if (!_dateTimeProvider.TryParse(time, date, out var dateTimeParseResult))
            {
                messageResult = null;
                return false;
            }

            messageResult = new Message(command, dateTimeParseResult!.Value, parts.Last());
            return true;
        }
        else
        {
            if (_dateTimeProvider.TryParse(date, date: null, out var dateTimeParseResult))
            {
                messageResult = new Message(command, dateTimeParseResult!.Value, parts.Last());
                return true;
            }
        }

        messageResult = null;

        return false;
    }
}