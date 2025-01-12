namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class ViewSchedulesCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.ViewSchedules;

    public string HelpDescription => throw new NotImplementedException();
}