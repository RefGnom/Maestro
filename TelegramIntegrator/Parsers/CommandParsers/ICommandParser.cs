using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers.CommandParsers;

public interface ICommandParser
{
    bool CanParse(string command);
    ParseResult<ICommand> ParseCommand(string command);
}