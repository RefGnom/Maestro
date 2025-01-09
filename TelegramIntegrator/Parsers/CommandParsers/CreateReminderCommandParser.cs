using Maestro.TelegramIntegrator.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maestro.TelegramIntegrator.Parsers.CommandParsers;

public class CreateReminderCommandParser(IDateTimeParser dateTimeParser) : ICommandParser
{
    private readonly IDateTimeParser _dateTimeProvider = dateTimeParser;

    public bool CanParse(string command)
    {
        return command.StartsWith("/reminder");
    }

    public ParseResult<ICommand> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        var telegramCommand = parts[0];
        var dateTime = ParserHelper.ParseDateTime(parts[1]);

        if (!dateTime.IsSuccessful)
            return ParseResult<ICommand>.CreateFailure(dateTime.ParseFailureMessage);

        var description = parts[2];
        var remindCount = 1;

        if (parts.Length == 4)
            {
                var parserIntResult = ParserHelper.ParseInt(parts[3]);
                if (!parserIntResult.IsSuccessful)
                    return ParseResult<ICommand>.CreateFailure(parserIntResult.ParseFailureMessage);
                remindCount = parserIntResult.Value;
        }

        return ParseResult<ICommand>.CreateSuccess(new CreateReminderCommand(telegramCommand, description, dateTime.Value!.Value, remindCount));
    }
}