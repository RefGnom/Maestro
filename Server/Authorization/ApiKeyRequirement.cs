using Microsoft.AspNetCore.Authorization;

namespace Maestro.Server.Authorization;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiKeyRequirement(string policy) : IAuthorizationRequirement
{
    public string Policy { get; } = policy;
}