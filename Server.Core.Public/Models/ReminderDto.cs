using System.ComponentModel.DataAnnotations;
using Maestro.Data.Core;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Maestro.Server.Public.Models;

public class ReminderDto
{
    [Required] public long UserId { get; set; }

    [Required]
    [MaxLength(DataConstraints.ReminderDescriptionMaxLength)]
    public string Description { get; set; }

    [Required] public DateTime ReminderTime { get; set; }

    [Required] public TimeSpan RemindInterval { get; set; }

    [Required] public int RemindCount { get; set; }
}