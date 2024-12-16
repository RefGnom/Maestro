namespace Maestro.Core.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }
}