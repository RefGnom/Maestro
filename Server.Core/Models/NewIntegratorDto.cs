using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Maestro.Server.Core.Models;

public class NewIntegratorDto
{
    [Required] public string ApiKey { get; set; }

    [Required] public string Role { get; set; }
}