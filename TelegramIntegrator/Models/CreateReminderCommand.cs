namespace Maestro.TelegramIntegrator.Models;

public class CreateReminderCommand(string command, string description, DateTime reminderTime, int remindCount) : ICommand
{
    public string Command { get; } = command;
    public string Description { get; } = description;
    public DateTime ReminderTime { get; } = reminderTime;
    public int RemindCount { get; } = remindCount;
}