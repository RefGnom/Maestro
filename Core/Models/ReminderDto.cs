using System.ComponentModel.DataAnnotations;
using Maestro.Data;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Core.Models;

public record ReminderDto
{
    public long UserId { get; init; }

    [MaxLength(DataConstraints.ReminderDescriptionMaxLength)]
    public string Description { get; init; }

    public DateTime ReminderTime { get; init; }

    public TimeSpan ReminderTimeDuration { get; init; }

    public bool IsRepeatable { get; init; }

    public bool IsCompleted { get; init; }

    public static ReminderDto Create(
        long userId,
        string description,
        DateTime reminderTime,
        TimeSpan reminderTimeDuration,
        bool isRepeatable
    )
    {
        return new ReminderDto
        {
            UserId = userId, Description = description, ReminderTime = reminderTime, ReminderTimeDuration = reminderTimeDuration,
            IsRepeatable = isRepeatable, IsCompleted = false
        };
    }
}