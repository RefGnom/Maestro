using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateScheduleCommandParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateSchedule;

    public override ParseResult<ICommandModel> ParseCommand(string command, DateTime messageDateTime)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 4)
        {
            return ParseResult.CreateFailure<ICommandModel>(
                TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateScheduleCommandPattern)
            );
        }

        DateTime startDateTime;
        var dateTimePart = parts[1];

        var fullDateTimeParseResult = ParserHelper.ParseDateTime(dateTimePart);
        if (fullDateTimeParseResult.IsSuccessful)
        {
            startDateTime = fullDateTimeParseResult.Value!.Value;
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
                    startDateTime = parseResult.Value!.Value;
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
                    startDateTime = parseResult.Value!.Value;
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

        var duration = ParserHelper.ParseTimeSpan(parts[2]);

        if (!duration.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(duration.ParseFailureMessage);
        }

        var description = parts[3];
        var canOverlap = parts.Length == 5 && parts[4] == "overlap";

        return ParseResult.CreateSuccess<ICommandModel>(
            new CreateScheduleCommandModel(
                startDateTime,
                duration.Value,
                description,
                canOverlap
            )
        );
    }
}