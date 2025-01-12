using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public class CreateReminderCommandParser : ICommandParser
{
    public string TelegramCommandName => TelegramCommandNames.CreateReminderTelegramCommand;

    public bool CanParse(string command)
    {
        return command.StartsWith(TelegramCommandName);
    }

    public ParseResult<ICommand> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 3)
        {
            return ParseResult<ICommand>.CreateFailure(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern));
        }

        var telegramCommand = parts[0];
        var dateTimeParseResult = ParserHelper.ParseDateTime(parts[1]);

        if (!dateTimeParseResult.IsSuccessful)
        {
            return ParseResult<ICommand>.CreateFailure(dateTimeParseResult.ParseFailureMessage);
        }

        var description = parts[2];
        var remindCount = 1;
        var remindInterval = TimeSpan.FromMinutes(5);

        if (parts.Length > 3)
        {
            var parserIntResult = ParserHelper.ParseInt(parts[3]);
            if (!parserIntResult.IsSuccessful)
            {
                return ParseResult<ICommand>.CreateFailure(parserIntResult.ParseFailureMessage);
            }

            remindCount = parserIntResult.Value;
        }

        if (parts.Length > 4)
        {
            var parserTimeSpanResult = ParserHelper.ParseTimeSpan(parts[4]);
            if (!parserTimeSpanResult.IsSuccessful)
            {
                return ParseResult<ICommand>.CreateFailure(parserTimeSpanResult.ParseFailureMessage);
            }

            remindInterval = parserTimeSpanResult.Value;
        }

        return ParseResult<ICommand>.CreateSuccess(new CreateReminderCommand(telegramCommand, description, dateTimeParseResult.Value!.Value, remindCount, remindInterval));
    }
}