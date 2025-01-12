using System.ComponentModel.DataAnnotations;
using Maestro.Server.Private.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Maestro.Server.Private.Models;

public class NewIntegratorDto
{
    [Required] public string ApiKey { get; set; }

    [Required] [RoleValidation] public string Role { get; set; }
}