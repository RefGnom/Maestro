using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers.CommandParsers;

public class CreateReminderCommandParser() : ICommandParser
{
    public bool CanParse(string command)
    {
        return command.StartsWith("/reminder");
    }

    public ParseResult<ICommand> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        var telegramCommand = parts[0];
        var dateTimeParseResult = ParserHelper.ParseDateTime(parts[1]);

        if (!dateTimeParseResult.IsSuccessful)
            return ParseResult<ICommand>.CreateFailure(dateTimeParseResult.ParseFailureMessage);

        var description = parts[2];
        var remindCount = 1;

        if (parts.Length > 3)
        {
            var parserIntResult = ParserHelper.ParseInt(parts[3]);
            if (!parserIntResult.IsSuccessful)
                return ParseResult<ICommand>.CreateFailure(parserIntResult.ParseFailureMessage);
            remindCount = parserIntResult.Value;
        }

        var remindInterval = TimeSpan.FromMinutes(5);

        if (parts.Length > 4)
        {
            var parserTimeSpanResult = ParserHelper.ParseTimeSpan(parts[4]);
            if (!parserTimeSpanResult.IsSuccessful)
                return ParseResult<ICommand>.CreateFailure(parserTimeSpanResult.ParseFailureMessage);
            remindInterval = parserTimeSpanResult.Value;
        }

        return ParseResult<ICommand>.CreateSuccess(new CreateReminderCommand(telegramCommand, description, dateTimeParseResult.Value!.Value, remindCount, remindInterval));
    }
}