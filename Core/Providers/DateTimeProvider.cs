using System.Globalization;
using Core.Result;

namespace Core.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    private const string timeFormat = "HH:mm";
    private const string dateTimeFormat = $"yyyy.MM.dd {timeFormat}";

    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }

    public Result<DateTime> TryParse(string time, string? date = null)
    {
        if (date is not null && DateTime.TryParseExact(
                $"{date} {time}",
                dateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result)
           )
        {
            return Result.Result.CreateSuccess(result);
        }

        return DateTime.TryParseExact(time, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeResult)
            ? Result.Result.CreateSuccess(timeResult > GetCurrentDateTime() ? timeResult : timeResult.AddDays(1))
            : Result.Result.CreateFailure<DateTime>($"Failed to parse DateTime (Time: {time}, Date: {date})");
    }
}