using System.ComponentModel.DataAnnotations;
using Maestro.Data.Core;

namespace Maestro.Server.Public.Models.Schedules;

public class ScheduleDto
{
    [Required] public long UserId { get; set; }

    [Required]
    [MaxLength(DataConstraints.DescriptionMaxLength)]
    public string Description { get; set; } = default!;

    [Required] public DateTime StartDateTime { get; set; }

    [Required] public DateTime EndDateTime { get; set; }

    [Required] public bool CanOverlap { get; set; }
}