// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models.Reminders;

public class ReminderIdDto
{
    [Required] public long ReminderId { get; set; }
}