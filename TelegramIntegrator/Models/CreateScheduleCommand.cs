namespace Maestro.TelegramIntegrator.Models;

public class CreateScheduleCommand(
    string command,
    string description,
    DateTime startDateTime,
    DateTime endDateTime,
    bool canOverlap
) : ICommand
{
    public string Command { get; } = command;
    public string Description { get; } = description;
    public DateTime StartDateTime { get; } = startDateTime;
    public DateTime EndDateTime { get; } = endDateTime;
    public bool CanOverlap { get; } = canOverlap;
}