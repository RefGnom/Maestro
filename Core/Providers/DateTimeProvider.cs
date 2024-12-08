using System.Globalization;
using Maestro.Core.Result;

namespace Maestro.Core.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    private const string TimeFormat = "HH:mm";
    private const string DateTimeFormat = $"yyyy.MM.dd {TimeFormat}";

    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }

    public Result<DateTime> TryParse(string time, string? date = null)
    {
        if (date is not null && DateTime.TryParseExact(
                $"{date} {time}",
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result)
           )
        {
            return Result.Result.CreateSuccess(result);
        }

        return DateTime.TryParseExact(time, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeResult)
            ? Result.Result.CreateSuccess(timeResult > GetCurrentDateTime() ? timeResult : timeResult.AddDays(1))
            : Result.Result.CreateFailure<DateTime>($"Failed to parse DateTime (Time: {time}, Date: {date})");
    }
}