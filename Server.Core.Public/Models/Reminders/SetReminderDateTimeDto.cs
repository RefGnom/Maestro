using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models.Reminders;

public class SetReminderDateTimeDto
{
    [Required]
    public long ReminderId { get; set; }
    
    [Required]
    public DateTime DateTime { get; set; }
}