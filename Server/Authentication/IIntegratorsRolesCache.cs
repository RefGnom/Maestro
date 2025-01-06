using System.Collections.Immutable;

namespace Maestro.Server.Authentication;

public interface IIntegratorsRolesCache
{
    void Set(long integratorId, List<string> roles);
    bool TryGetRoles(long integratorId, out IImmutableList<string>? roles);
}