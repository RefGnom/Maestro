namespace Maestro.TelegramIntegrator.Parsers;

public interface IDateTimeParser
{
    bool TryParse(string time, string? date, out DateTime? result);
}