using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Parsers;

public class ViewRemindersCommandParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.ViewReminders;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        return ParseResult.CreateSuccess<ICommandModel>(new ViewRemindersCommandModel());
    }
}