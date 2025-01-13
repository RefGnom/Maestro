namespace Maestro.TelegramIntegrator.Implementation.ParsHelpers;

public interface IDateTimeParser
{
    bool TryParse(string dateTime, out DateTime? dateTimeResult);
}