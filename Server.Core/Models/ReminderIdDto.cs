// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Core.Models;

public class ReminderIdDto
{
    [Required] public long Id { get; set; }
}