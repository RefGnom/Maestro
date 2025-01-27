﻿namespace Maestro.TelegramIntegrator.Implementation.Commands;

public static class TelegramCommandNames
{
    public const string CreateReminder = "/reminder";
    public const string CreateReminderStepByStep = "/reminder_sbs";
    public const string CreateSchedule = "/schedule";
    public const string CreateScheduleStepByStep = "/schedule_sbs";
    public const string ViewReminders = "/view_reminders";
    public const string ViewSchedules = "/view_schedules";
    public const string SetTimeZone = "/set_timezone";

    public static string[] GetCommandNames()
    {
        var type = typeof(TelegramCommandNames);
        var fieldInfos = type.GetFields();
        return fieldInfos.Select(x => x.GetValue(null)!.ToString()).ToArray()!;
    }
}