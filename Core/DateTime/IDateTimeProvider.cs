namespace Core.DateTime;

public interface IDateTimeProvider
{
    System.DateTime GetCurrentDateTime();
    bool TryParseDateTime(string time, string? date, out System.DateTime dateTime);
}