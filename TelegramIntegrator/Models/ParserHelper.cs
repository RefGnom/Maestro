using Maestro.TelegramIntegrator.Parsers;

namespace Maestro.TelegramIntegrator.Models
{
    public class ParserHelper
    {
        public static ParseResult<DateTime?> ParseDateTime(string dateTime)
        {
            return new DateTimeParser().TryParse(dateTime, out var dateTimeParseResult)
                ? ParseResult<DateTime?>.CreateSuccess(dateTimeParseResult)
                : ParseResult<DateTime?>.CreateFailure("Не удалось распарсить дату или время. Напишите дату в формате \"день.месяц.год\", а время в формате \"часы:минуты\".");
        }

        public static ParseResult<int> ParseInt(string integer)
        {
            return int.TryParse(integer, out var parseInteger)
                ? ParseResult<int>.CreateSuccess(parseInteger)
                : ParseResult<int>.CreateFailure("Не удалось распарсить количество отправки напоминания. Напишите целое число.");
        }
    }
}
