using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Maestro.Server.Public.Models.Schedules;

public class ScheduleDto
{
    [Required] public long UserId { get; set; }
    
    [Required] public string Description { get; set; }

    [Required] public DateTime StartDateTime { get; set; }

    [Required] public TimeSpan Duration { get; set; }

    [Required] public bool CanOverlap { get; set; }

    [Required] public bool IsCompleted { get; set; }
}