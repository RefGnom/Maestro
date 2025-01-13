using System.ComponentModel.DataAnnotations;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Private.Models;

public class CompletedRemindersDto
{
    public const int LimitMaxValue = 50;
    
    [Required] [Range(0, LimitMaxValue)] public int Limit { get; set; }

    [Required] public int Offset { get; set; }
}