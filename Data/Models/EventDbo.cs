namespace Maestro.Data.Models;

public record EventDbo(
    Guid Id,
    long UserId,
    long IntegratorId,
    string Description,
    DateTime ReminderTime,
    DateTime ReminderTimeDuration,
    bool IsCompleted = false
);