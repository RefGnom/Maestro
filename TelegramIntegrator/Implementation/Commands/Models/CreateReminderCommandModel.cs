using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateReminderCommandModel(
    DateTime reminderTime,
    string description,
    int remindCount,
    TimeSpan remindInterval
) : ICommandModel
{
    public DateTime ReminderTime { get; } = reminderTime;
    public string ReminderDescription { get; } = description;
    public int RemindCount { get; } = remindCount;
    public TimeSpan RemindInterval { get; } = remindInterval;
    public string TelegramCommand => TelegramCommandNames.CreateReminder;

    public string HelpDescription => TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern);
}
