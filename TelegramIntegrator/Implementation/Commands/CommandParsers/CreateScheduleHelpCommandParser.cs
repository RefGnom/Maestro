using Maestro.TelegramIntegrator.Implementation.Commands.CommandsModels;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CommandParsers;

public class CreateScheduleHelpCommandParser : CommandParserBase
{
    public override string Name => TelegramCommandNames.CreateScheduleHelp;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        return ParseResult.CreateSuccess<ICommandModel>(new CreateScheduleHelpCommandModel());
    }
}