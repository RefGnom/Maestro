using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateScheduleCommandParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateSchedule;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        if (parts.Length < 4)
        {
            return ParseResult.CreateFailure<ICommandModel>(
                TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateScheduleCommandPattern)
            );
        }

        var startDateTime = ParserHelper.ParseDateTime(parts[1]);

        if (!startDateTime.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(startDateTime.ParseFailureMessage);
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
                startDateTime.Value!.Value,
                duration.Value,
                description,
                canOverlap
            )
        );
    }
}