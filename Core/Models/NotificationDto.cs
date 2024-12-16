namespace Maestro.Core.Models;

public record NotificationDto(
    long Id,
    long UserId,
    string Description,
    DateTime ReminderTime,
    TimeSpan ReminderTimeDuration,
    bool IsRepeatable,
    bool IsCompleted
)
{
    public static NotificationDto Create(
        long userId,
        string description,
        DateTime reminderTime,
        TimeSpan reminderTimeDuration,
        bool isRepeatable
    )
    {
        return new NotificationDto(0, userId, description, reminderTime, reminderTimeDuration, isRepeatable, false);
    }
}