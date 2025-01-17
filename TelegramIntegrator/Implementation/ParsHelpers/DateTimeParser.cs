using Maestro.Core.Providers;
using System.Globalization;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class DateTimeParser : IDateTimeParser
{
    private string[] timeFormats = new[] { "HH:mm", "HH.mm", "HH" };
    private const string DateTimeFormat = $"dd.MM.yyyy HH:mm";

    public bool TryParse(string dateTime, DateTime messageDateTime, out DateTime? dateTimeResult)
    {
        if (DateTime.TryParseExact(
                dateTime,
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

        foreach ( var timeFormat in timeFormats )
        {
            if (DateTime.TryParseExact(
                dateTime,
                timeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var timeResult))
            {
                dateTimeResult = timeResult > messageDateTime ? timeResult : timeResult.AddDays(1);
                return true;
            }
        }

        dateTimeResult = null;
        return false;
    }
}