namespace Maestro.TelegramIntegrator.Parsers;

public interface IDateTimeParser
{
    bool TryParse(string dateTime, out DateTime? dateTimeResult);
}