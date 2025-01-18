using Maestro.TelegramIntegrator.Implementation.Commands.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.SetTimeZone;

public class SetTimeZoneCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.SetTimeZone;
    public string HelpDescription => throw new NotSupportedException("Не поддерживается помощь для этой команды");
}