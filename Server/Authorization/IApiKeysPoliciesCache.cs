using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Maestro.Server.Authorization;

public interface IApiKeysPoliciesCache
{
    void Set(long integratorId, List<string> policies);
    bool TryGetPolicies(long integratorId, out IImmutableList<string>? entry);
}