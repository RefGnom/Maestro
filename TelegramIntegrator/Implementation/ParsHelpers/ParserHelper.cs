using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public class ParserHelper
{
    public static ParseResult<DateTime?> ParseDateTime(string dateTime)
    {
        return new DateTimeParser().TryParse(dateTime, out var parsedDateTime)
            ? ParseResult<DateTime?>.CreateSuccess(parsedDateTime)
            : ParseResult<DateTime?>.CreateFailure("Не удалось распарсить дату или время. Напишите дату в формате \"день.месяц.год\", а время в формате \"часы:минуты\".");
    }

    public static ParseResult<int> ParseInt(string integer)
    {
        return int.TryParse(integer, out var parsedInteger)
            ? ParseResult<int>.CreateSuccess(parsedInteger)
            : ParseResult<int>.CreateFailure("Не удалось распарсить количество отправки напоминания. Напишите целое число.");
    }

    public static ParseResult<TimeSpan> ParseTimeSpan(string timeSpan)
    {
        return TimeSpan.TryParse(timeSpan, out var parsedTimeSpan)
            ? ParseResult<TimeSpan>.CreateSuccess(parsedTimeSpan)
            : ParseResult<TimeSpan>.CreateFailure("Не удалось распарсить интервал повторной отправки напоминания. Напишите время в формате \"дни:часы:минуты:секунды\".");
    }
}