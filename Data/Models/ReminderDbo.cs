using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Data.Models;

public class ReminderDbo
{
    public long Id { get; init; }

    public long UserId { get; init; }

    public long IntegratorId { get; init; }

    public string Description { get; init; }

    public DateTime ReminderTime { get; init; }

    public DateTime ReminderTimeDuration { get; init; }

    public bool IsRepeatable { get; init; }

    public bool IsCompleted { get; init; }
}