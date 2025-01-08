namespace Maestro.TelegramIntegrator.Models;

public class ScheduleCommand : ICommand
{
    public string Command { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public bool CanOverlap { get; }

    public ScheduleCommand(string command, string description, DateTime startDateTime, DateTime endDateTime, bool canOverlap)
    {
        Command = command;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        CanOverlap = canOverlap;
    }
}