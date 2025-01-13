﻿namespace Maestro.TelegramIntegrator.View;

public static class TelegramCommandPatterns
{
    public const string CreateReminderCommandPattern = "/reminder, дата время, описание, сколько раз напомнить (по умолчанию 1), интервал dd:hh:mm (по умолчанию 5 минут)";
    public const string CreateScheduleCommandPattern = "/schedule, дата время начала, время (продолжительность расписания) dd:hh:mm, описание, overlap - если это расписание может пересекаться с другими, иначе ничего не пишите";
}