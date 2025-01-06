namespace Maestro.Server.Repositories;

public interface IIntegratorsRolesRepository
{
    Task<List<string>> GetIntegratorRolesAsync(long integratorId, CancellationToken cancellationToken);
    Task AddIntegratorRoleAsync(long integratorId, string role, CancellationToken cancellationToken);
}