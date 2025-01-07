// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Public.Models;

public class ReminderIdDto
{
    [Required] public long Id { get; set; }
}