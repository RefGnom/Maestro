using System.Globalization;
using Maestro.Core.Providers;

namespace Maestro.TelegramIntegrator.Parsers;

public class DateTimeParser(IDateTimeProvider dateTimeProvider) : IDateTimeParser
{
    private const string TimeFormat = "HH:mm";

    private const string DateTimeFormat = $"yyyy.MM.dd {TimeFormat}";

    public bool TryParse(string time, string? date, out DateTime? dateTimeResult)
    {
        if (date is not null && DateTime.TryParseExact(
                $"{date} {time}",
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result
            )
           )
        {
            dateTimeResult = result;
            return true;
        }

        if (!DateTime.TryParseExact(
                time,
                TimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var timeResult
            ))
        {
            dateTimeResult = null;
            return false;
        }

        dateTimeResult = timeResult > dateTimeProvider.GetCurrentDateTime() ? timeResult : timeResult.AddDays(1);
        return true;
    }
}