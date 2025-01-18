using Maestro.TelegramIntegrator.Implementation.Commands.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.SetTimeZone;

public class SetTimeZoneCommandModel() : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.SetTimeZone;
    public string HelpDescription => "Введите дельту от международного UTC времени. Москва: +3";
}