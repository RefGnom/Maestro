namespace Maestro.Client.Models;

public record EventDto(
    Guid Id,
    long UserId,
    long IntegratorId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsCompleted = false
);