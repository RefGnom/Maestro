using System.Globalization;

namespace Core.Providers;

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
            return Result.CreateSuccess(result);
        }

        if (DateTime.TryParseExact(time, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var timeResult))
        {
            return Result.CreateSuccess(timeResult > GetCurrentDateTime() ? timeResult : timeResult.AddDays(1));
        }

        return Result.CreateFailure<DateTime>($"Не удалось распарсить дату time: {time} date: {date}");
    }
}