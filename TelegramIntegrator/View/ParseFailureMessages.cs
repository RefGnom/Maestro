namespace Maestro.TelegramIntegrator.View;

public class ParseFailureMessages
{
    public const string ParseDateTimeFailureMessage =
        "дата или время. Напишите дату в формате \"день.месяц.год\", а время в формате \"часы:минуты\".";
    public const string ParseIntFailureMessage =
        "количество отправки напоминания. Напишите целое натуральное число.";
    public const string ParseTimeSpanFailureMessage =
        "интервал повторной отправки напоминания. Напишите время в формате \"дни:часы:минуты:секунды\".";
}