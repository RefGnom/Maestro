using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Core.Models;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
public class ReminderWithIdDto : ReminderDto
{
    [Required] public long Id { get; set; }
}