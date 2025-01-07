using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models;

public class NewReminderDateTimeDto
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public DateTime NewDateTime { get; set; }
}