using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Core.ApiModels;

public class RemindersForUserDto
{
    public const int LimitMaxValue = 50;
    
    public long UserId { get; set; }
    
    public int Offset { get; set; }
    
    [Range(0, LimitMaxValue)]
    public int Limit { get; set; }
}