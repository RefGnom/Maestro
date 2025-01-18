using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateScheduleCommandParser(IDateTimeProvider dateTimeProvider) : CommandParserBase
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

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

        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        var startDateTimeParseResult = ParserHelper.ParseDateTime(parts[1], currentDateTime);

        if (!startDateTimeParseResult.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(startDateTimeParseResult.ParseFailureMessage);
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
                startDateTimeParseResult.Value!.Value,
                duration.Value,
                description,
                canOverlap
            )
        );
    }
}