namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class ViewRemindersCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.ViewReminders;

    public string HelpDescription => throw new NotImplementedException();
}