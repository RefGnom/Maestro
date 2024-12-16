namespace Maestro.Data.Models;

public record ReminderDbo(
    long Id,
    long UserId,
    long IntegratorId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsRepeatable,
    bool IsCompleted
);