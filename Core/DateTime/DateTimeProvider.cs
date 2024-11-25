using System.Globalization;

namespace Core.DateTime;

public class DateTimeProvider : IDateTimeProvider
{
    private static readonly string TimeFormat = "HH:mm";
    private static readonly string DateTimeFormat = $"yyyy.MM.dd {TimeFormat}";

    public System.DateTime GetCurrentDateTime()
    {
        return System.DateTime.Now;
    }

    public bool TryParseDateTime(string time, string? date, out System.DateTime dateTime)
    {
        if (date is not null && System.DateTime.TryParseExact($"{date} {time}", DateTimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
        {
            dateTime = result;
            return true;
        }

        if (System.DateTime.TryParseExact(time, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var timeResult))
        {
            if (timeResult > GetCurrentDateTime())
            {
                dateTime = timeResult;
                return true;
            }

            dateTime = timeResult.AddDays(1);
            return true;
        }

        dateTime = default;
        return false;
    }
}