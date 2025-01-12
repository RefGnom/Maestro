using Maestro.Server.Repositories.Results.IntegratorsRoles;

namespace Maestro.Server.Repositories;

public interface IIntegratorsRolesRepository
{
    Task<GetIntegratorRolesRepositoryResult> GetIntegratorRoleAsync(long integratorId, CancellationToken cancellationToken);
    Task AddIntegratorRoleAsync(long integratorId, string role, CancellationToken cancellationToken);
}