using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Public.Models;

public class RemindersForUserDto
{
    public const int LimitMaxValue = 50;

    [Required] public long UserId { get; set; }

    [Range(0, int.MaxValue)] public int Offset { get; set; }

    [Range(0, LimitMaxValue)] public int Limit { get; set; }

    public DateTime? ExclusiveStartDateTime { get; set; }
}