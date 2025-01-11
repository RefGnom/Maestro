using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Private.Models;

public class OldRemindersDto
{
    public const int LimitMaxValue = 50;
    
    [Required] [Range(0, LimitMaxValue)] public int Limit { get; set; }

    [Required] public int Offset { get; set; }
    
    [Required] public DateTime InclusiveBeforeDateTime { get; set; }
}