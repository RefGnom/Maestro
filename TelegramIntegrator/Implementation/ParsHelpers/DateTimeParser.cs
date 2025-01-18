using System.Globalization;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class DateTimeParser : IDateTimeParser
{
    private readonly string[] _dateTimeFormats = ["dd.MM.yyyy HH:mm", "dd.MM.yyyy HH.mm", "dd.MM.yyyy HH"];

    public bool TryParse(string dateTime, DateTime messageDateTime, out DateTime? dateTimeResult)
    {
        foreach (var dateTimeFormat in _dateTimeFormats)
        {
            if (DateTime.TryParseExact(
                    dateTime,
                    dateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var result
                )
               )
            {
                dateTimeResult = result > messageDateTime ? result : result.AddDays(1);
                return true;
            }
        }

        dateTimeResult = null;
        return false;
    }
}