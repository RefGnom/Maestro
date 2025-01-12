namespace Maestro.TelegramIntegrator.Implementation.View;

public static class TelegramCommandPatterns
{
    public const string CreateReminderCommandPattern = "/reminder, дата время, описание, сколько раз напомнить (по умолчанию 1), интервал dd:hh:mm:ss (по умолчанию 5 минут)";
    public const string CreateScheduleCommandPattern =
        "/schedule, дата время начала расписания, дата время конца расписания, описание, overlap (если расписание может пересекаться с другими)";
}