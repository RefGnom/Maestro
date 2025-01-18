using Maestro.TelegramIntegrator.Implementation.Commands.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class CreateScheduleStepByStepCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.CreateScheduleStepByStep;
    public string HelpDescription => throw new NotSupportedException("Не поддерживается помощь для этой команды");
}