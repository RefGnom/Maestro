using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation;

public interface IUserDateTimeService
{
    bool TryGetUserDateTime(long userId, out DateTime? userDateTime);
    void SetUserLocation(long userId, Location location);
}