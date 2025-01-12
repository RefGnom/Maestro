using Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public class CreateReminderCommandParser : CommandParserBase
{
    public override string Name => TelegramCommandNames.CreateReminder;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 3)
        {
            return ParseResult.CreateFailure<ICommandModel>(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern));
        }

        var dateTimeParseResult = ParserHelper.ParseDateTime(parts[1]);

        if (!dateTimeParseResult.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(dateTimeParseResult.ParseFailureMessage);
        }

        var description = parts[2];
        var remindCount = 1;
        var remindInterval = TimeSpan.FromMinutes(5);

        if (parts.Length > 3)
        {
            var parserIntResult = ParserHelper.ParseInt(parts[3]);
            if (!parserIntResult.IsSuccessful)
            {
                return ParseResult.CreateFailure<ICommandModel>(parserIntResult.ParseFailureMessage);
            }

            remindCount = parserIntResult.Value;
        }

        if (parts.Length > 4)
        {
            var parserTimeSpanResult = ParserHelper.ParseTimeSpan(parts[4]);
            if (!parserTimeSpanResult.IsSuccessful)
            {
                return ParseResult.CreateFailure<ICommandModel>(parserTimeSpanResult.ParseFailureMessage);
            }

            remindInterval = parserTimeSpanResult.Value;
        }

        return ParseResult.CreateSuccess<ICommandModel>(
            new CreateReminderCommandModel(
                dateTimeParseResult.Value!.Value,
                description,
                remindCount,
                remindInterval
            )
        );
    }
}