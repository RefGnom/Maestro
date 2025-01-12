﻿using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public interface ICommandParser
{
    string Name { get; }
    bool CanParse(string command);
    ParseResult<ICommandModel> ParseCommand(string command);
}