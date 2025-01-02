// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Maestro.Data.Models;

public class IntegratorApiKeyDbo
{
    public long Id { get; set; }

    public long IntegratorId { get; set; }

    public string ApiKey { get; set; }

    public ApiKeyState State { get; set; }
}