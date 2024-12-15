namespace Maestro.Core.Models;

public record EventDto(
    long UserId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsCompleted = false,
    bool IsRepeatable = false)
{
    public long Id { get; init; }
}