﻿using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateScheduleCommandParser : CommandParserBase
{
    public override string Name => TelegramCommandNames.CreateSchedule;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        var parts = command.Split(",", StringSplitOptions.TrimEntries);
        var startDateTime = ParserHelper.ParseDateTime(parts[1]);

        if (!startDateTime.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(startDateTime.ParseFailureMessage);
        }

        var endDateTime = ParserHelper.ParseDateTime(parts[2]);

        if (!endDateTime.IsSuccessful)
        {
            return ParseResult.CreateFailure<ICommandModel>(endDateTime.ParseFailureMessage);
        }

        var description = parts[3];
        var canOverlap = parts.Length == 5 && parts[4] == "overlap";

        return ParseResult.CreateSuccess<ICommandModel>(
            new CreateScheduleCommandModel(
                startDateTime.Value!.Value,
                endDateTime.Value!.Value,
                description,
                canOverlap
            )
        );
    }
}