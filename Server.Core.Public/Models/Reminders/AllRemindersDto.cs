using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Public.Models.Reminders;

public class AllRemindersDto
{
    public const int LimitMaxValue = 50;

    [Required] public DateTime ExclusiveStartDateTime { get; set; }

    [Required] [Range(0, LimitMaxValue)] public int Limit { get; set; }

    [Required] public int Offset { get; set; }
}