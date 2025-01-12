using Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public interface ICommandParser
{
    string Name { get; }
    bool CanParse(string command);
    ParseResult<ICommandModel> ParseCommand(string command);
}