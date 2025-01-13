﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Maestro.Data.Models;

public class ScheduleDbo
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long IntegratorId { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool CanOverlap { get; set; }
    public bool IsCompleted { get; set; }
}