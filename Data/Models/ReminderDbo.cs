﻿// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Data.Models;

public class ReminderDbo
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long IntegratorId { get; set; }

    public string Description { get; set; }

    public DateTime RemindDateTime { get; set; }

    public TimeSpan RemindInterval { get; set; }

    public int RemindCount { get; set; }

    public bool IsCompleted { get; set; }
}