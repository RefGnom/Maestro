using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Parsers;

public interface ICommandParser
{
    ParseResult ParseCommand(string command);
}