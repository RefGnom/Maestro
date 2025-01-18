﻿using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class CreateScheduleHelpCommandParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateScheduleHelp;

    public override ParseResult<ICommandModel> ParseCommand(string command, DateTime messageDateTime)
    {
        return ParseResult.CreateSuccess<ICommandModel>(new CreateScheduleHelpCommandModel());
    }
}