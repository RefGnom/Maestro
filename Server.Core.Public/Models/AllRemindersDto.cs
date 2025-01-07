using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models;

public class AllRemindersDto
{
    public const int LimitMaxValue = 50;
    
    [Required]
    public DateTime ExclusiveStartDateTime { get; set; }
    
    [Required]
    [Range(0, LimitMaxValue)]
    public int Limit { get; set; }
    
    [Required]
    public int Offset { get; set; }
}