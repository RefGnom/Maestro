using Maestro.TelegramIntegrator.Implementation.Commands.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class CreateReminderStepByStepCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.CreateReminderStepByStep;
    public string HelpDescription => throw new NotSupportedException("Не поддерживается помощь для этой команды");
}