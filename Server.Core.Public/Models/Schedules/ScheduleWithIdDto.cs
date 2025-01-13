using System.ComponentModel.DataAnnotations;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Public.Models.Schedules;

public class ScheduleWithIdDto : ScheduleDto
{
    [Required] public long ScheduleId { get; set; }
}