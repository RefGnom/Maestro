namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public interface IDateTimeParser
{
    bool TryParse(string dateTime, DateTime messageDateTime, out DateTime? dateTimeResult);
}