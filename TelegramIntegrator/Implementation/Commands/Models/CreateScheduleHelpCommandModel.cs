using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateScheduleHelpCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.CreateSchedule;
    public string HelpDescription => TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateScheduleCommandPattern);
}
