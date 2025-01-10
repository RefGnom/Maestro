using System.Globalization;

namespace Maestro.TelegramIntegrator.Parsers;

public class DateTimeParser() : IDateTimeParser
{
    private const string DateTimeFormat = $"dd.MM.yyyy HH:mm";

    public bool TryParse(string dateTime, out DateTime? dateTimeResult)
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

        dateTimeResult = null;
        return false;
    }
}