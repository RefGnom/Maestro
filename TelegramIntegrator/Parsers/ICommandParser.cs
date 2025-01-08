using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public interface ICommandParser
{
    bool TryParseCommand(string command, out ICommand? commandResult);
}