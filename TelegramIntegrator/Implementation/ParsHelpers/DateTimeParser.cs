using Maestro.Core.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class DateTimeParser : IDateTimeParser
{
    private string[] dateTimeFormats = new[] { "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH.mm", "dd.MM.yyyy HH" };

    public bool TryParse(string dateTime, DateTime messageDateTime, out DateTime? dateTimeResult)
    {
        foreach (var dateTimeFormat in dateTimeFormats)
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