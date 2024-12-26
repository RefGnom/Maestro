#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Maestro.Data.Models;

public class ApiKeyDbo
{
    public long Id { get; init; }
    
    public string Key { get; init; }
    
    public long IntegratorId { get; init; }
    
    public ApiKeyState State { get; init; }
}