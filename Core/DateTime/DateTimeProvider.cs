namespace Core.DateTime;

public class DateTimeProvider : IDateTimeProvider
{
    public System.DateTime GetCurrentDateTime()
    {
        return System.DateTime.Now;
    }
}