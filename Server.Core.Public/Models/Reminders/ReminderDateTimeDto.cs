using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Maestro.Server.Public.Models.Reminders;

public class ReminderDateTimeDto
{
    [Required] public long ReminderId { get; set; }

    [Required] public DateTime DateTime { get; set; }
}