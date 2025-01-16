using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateReminderCommandParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateReminder;

    public override ParseResult<ICommandModel> ParseCommand(string command, DateTime messageDateTime)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 3)
        {
            return ParseResult.CreateFailure<ICommandModel>(TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern));
        }

        DateTime reminderDateTime;
        var dateTimePart = parts[1];

        var fullDateTimeParseResult = ParserHelper.ParseDateTime(dateTimePart);
        if (fullDateTimeParseResult.IsSuccessful)
        {
            reminderDateTime = fullDateTimeParseResult.Value!.Value;
        }
        else
        {
            var dateParts = dateTimePart.Split(' ');
            if (dateParts.Length == 2)
            {
                var date = dateParts[0];
                var time = dateParts[1];
                var parseResult = ParserHelper.ParseDateTime($"{date}.{messageDateTime.Year} {time}");

                if (parseResult.IsSuccessful)
                {
                    reminderDateTime = parseResult.Value!.Value;
                }
                else
                {
                    return ParseResult.CreateFailure<ICommandModel>(parseResult.ParseFailureMessage);
                }
            }
            else if (dateParts.Length == 1)
            {
                var parseResult = ParserHelper.ParseDateTime($"{messageDateTime.Date:dd.MM.yyyy} {dateParts[0]}");
                if (parseResult.IsSuccessful)
                {
                    reminderDateTime = parseResult.Value!.Value;
                }
                else
                {
                    return ParseResult.CreateFailure<ICommandModel>(parseResult.ParseFailureMessage);
                }
            }
            else
            {
                return ParseResult.CreateFailure<ICommandModel>(TelegramMessageBuilder.BuildParseFailureMessage(ParseFailureMessages.ParseDateTimeFailureMessage));
            }
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
                reminderDateTime,
                description,
                remindCount,
                remindInterval
            )
        );
    }
}