using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public interface ICommandParser
{
    string TelegramCommandName { get; }
    bool CanParse(string command);
    ParseResult<ICommand> ParseCommand(string command);
}