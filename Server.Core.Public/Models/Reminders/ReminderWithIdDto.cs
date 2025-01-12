using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models.Reminders;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
public class ReminderWithIdDto : ReminderDto
{
    [Required] public long ReminderId { get; set; }
}