using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Core.Models;

public class RemindersForUserDto
{
    public const int LimitMaxValue = 50;

    public long UserId { get; set; }

    [Range(minimum: 0, int.MaxValue)] public int Offset { get; set; }

    [Range(minimum: 0, LimitMaxValue)] public int Limit { get; set; }
}