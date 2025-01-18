namespace Maestro.TelegramIntegrator.Implementation.Commands;

public static class TelegramCommandNames
{
    public const string CreateReminder = "/reminder";
    public const string CreateReminder
    public const string CreateSchedule = "/schedule";
    public const string CreateReminderHelp = "/reminder_h";
    public const string CreateScheduleHelp = "/schedule_h";
    public const string ViewReminders = "/view_reminders";
    public const string ViewSchedules = "/view_schedules";

    public static string[] GetCommandNames()
    {
        var type = typeof(TelegramCommandNames);
        var fieldInfos = type.GetFields();
        return fieldInfos.Select(x => x.GetValue(null)!.ToString()).ToArray()!;
    }
}