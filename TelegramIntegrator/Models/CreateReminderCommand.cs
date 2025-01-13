namespace Maestro.TelegramIntegrator.Models;

public class CreateReminderCommand(string command, string description, DateTime reminderTime, int remindCount, TimeSpan remindInterval) : ICommand
{
    public string Command { get; } = command;
    public string Description { get; } = description;
    public DateTime ReminderTime { get; } = reminderTime;
    public int RemindCount { get; } = remindCount;
    public TimeSpan RemindInterval { get; } = remindInterval;
}