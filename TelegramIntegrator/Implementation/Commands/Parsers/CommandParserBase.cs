using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public abstract class CommandParserBase : ICommandParser
{
    public abstract string CommandName { get; }
    public bool CanParse(string command) => command.StartsWith(CommandName);
    public abstract ParseResult<ICommandModel> ParseCommand(string command);
}