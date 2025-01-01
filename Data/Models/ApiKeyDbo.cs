// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Maestro.Data.Models;

public class ApiKeyDbo
{
    public long Id { get; set; }

    public string Key { get; set; }

    public long IntegratorId { get; set; }

    public ApiKeyState State { get; set; }
}