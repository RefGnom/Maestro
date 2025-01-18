using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateScheduleCommandModel(
    DateTime startDateTime,
    TimeSpan duration,
    string description,
    bool canOverlap
) : ICommandModel
{
    public DateTime StartDateTime { get; } = startDateTime;
    public TimeSpan Duration { get; } = duration;
    public string ScheduleDescription { get; } = description;
    public bool CanOverlap { get; } = canOverlap;
    public string TelegramCommand => TelegramCommandNames.CreateSchedule;

    public string HelpDescription => TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateScheduleCommandPattern);
}
