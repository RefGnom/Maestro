namespace Maestro.Data.Models;

public record NotificationDbo(
    long Id,
    long UserId,
    long IntegratorId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsRepeatable,
    bool IsCompleted
);