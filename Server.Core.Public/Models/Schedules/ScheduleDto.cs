using System.ComponentModel.DataAnnotations;
using Maestro.Data.Core;

<<<<<<<< HEAD:Server.Core.Public/Models/Reminders/ScheduleDto.cs
// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Server.Core.Models;
========
namespace Maestro.Server.Public.Models.Schedules;
>>>>>>>> ce0d37e (команды просмотра напоминаний/расписаний):Server.Core.Public/Models/Schedules/ScheduleDto.cs

public class ScheduleDto
{
    [Required] public long UserId { get; set; }

    [Required]
    [MaxLength(DataConstraints.DescriptionMaxLength)]
    public string Description { get; set; }

    [Required] public DateTime StartDateTime { get; set; }

    [Required] public DateTime EndDateTime { get; set; }

    [Required] public bool CanOverlap { get; set; }
}