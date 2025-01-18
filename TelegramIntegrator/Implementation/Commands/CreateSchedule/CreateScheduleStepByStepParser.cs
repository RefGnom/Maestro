using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Implementation.Commands.Parsers;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class CreateScheduleStepByStepParser : CommandParserBase
{
    public override string CommandName => TelegramCommandNames.CreateScheduleStepByStep;

    public override ParseResult<ICommandModel> ParseCommand(string command)
    {
        return ParseResult.CreateSuccess<ICommandModel>(new CreateScheduleStepByStepCommandModel());
    }
}