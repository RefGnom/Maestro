namespace Maestro.Data.Models;

public record EventDbo(
    long Id,
    long UserId,
    long IntegratorId,
    string Description,
    DateTime ReminderTime,
    DateTime ReminderTimeDuration,
    bool IsCompleted,
    bool IsRepeatable
);