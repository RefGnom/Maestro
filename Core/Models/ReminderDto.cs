namespace Maestro.Core.Models;

public record ReminderDto(
    long Id,
    long UserId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsRepeatable,
    bool IsCompleted
)
{
    public static ReminderDto Create(
        long userId,
        string description,
        DateTime reminderTime,
        TimeSpan reminderTimeDuration,
        bool isRepeatable
    )
    {
        return new ReminderDto(0, userId, description, reminderTime, reminderTimeDuration, isRepeatable, false);
    }
}