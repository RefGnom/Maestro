using GeoTimeZone;
using Telegram.Bot.Types;
using TimeZoneConverter;

namespace Maestro.TelegramIntegrator.Implementation;

public class UserDateTimeService : IUserDateTimeService
{
    private readonly Dictionary<long, TimeZoneInfo> _timeZones = new();

    public bool TryGetUserDateTime(long userId, out DateTime? userDateTime)
    {
        if (_timeZones.TryGetValue(userId, out var timeZoneInfo))
        {
            userDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
            return true;
        }

        userDateTime = null;
        return false;
    }

    public void SetUserLocation(long userId, Location location)
    {
        var timeZone = TimeZoneLookup.GetTimeZone(location.Latitude, location.Longitude).Result;
        var timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZone);
        _timeZones[userId] = timeZoneInfo;
    }
}