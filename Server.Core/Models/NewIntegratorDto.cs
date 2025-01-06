#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace Maestro.Server.Core.Models;

public class NewIntegratorDto
{
    public string ApiKey { get; set; }

    public string Role { get; set; }
}