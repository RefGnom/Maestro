using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class CreateReminderStepByStepParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateReminderStepByStep;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        return ParseResult.CreateSuccess<ICommandModel>(new CreateReminderStepByStepCommandModel());
    }
}