namespace Maestro.Server.Authentication;

public interface IIntegratorsRolesCache
{
    void Set(long integratorId, string role);
    bool TryGetRole(long integratorId, out string? role);
}