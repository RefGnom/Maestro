using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
public class ReminderWithIdDto : ReminderDto
{
    [Required] public long Id { get; set; }
}