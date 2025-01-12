using Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public abstract class CommandParserBase : ICommandParser
{
    public abstract string Name { get; }
    public bool CanParse(string command) => command.StartsWith(Name);
    public abstract ParseResult<ICommandModel> ParseCommand(string command);
}