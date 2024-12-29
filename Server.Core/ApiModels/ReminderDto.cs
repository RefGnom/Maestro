using System.ComponentModel.DataAnnotations;
using Maestro.Data.Core;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Server.Core.ApiModels;

public class ReminderDto
{
    public long UserId { get; set; }

    [MaxLength(DataConstraints.ReminderDescriptionMaxLength)]
    public string Description { get; set; }

    public DateTime ReminderTime { get; set; }

    public TimeSpan ReminderTimeDuration { get; set; }

    public bool IsRepeatable { get; set; }

    public bool IsCompleted { get; set; }

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
            UserId = userId,
            Description = description,
            ReminderTime = reminderTime,
            ReminderTimeDuration = reminderTimeDuration,
            IsRepeatable = isRepeatable,
            IsCompleted = false
        };
    }
}