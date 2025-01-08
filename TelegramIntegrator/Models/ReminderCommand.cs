namespace Maestro.TelegramIntegrator.Models;

public class ReminderCommand : ICommand
{
    public string Command { get; }
    public string Description { get; }
    public DateTime ReminderTime { get; }

    public ReminderCommand(string command, string description, DateTime reminderTime)
    {
        Command = command;
        Description = description;
        ReminderTime = reminderTime;
    }
}
