using Maestro.Server.Repositories.Results.IntegratorRoles;

namespace Maestro.Server.Repositories;

public interface IIntegratorRolesRepository
{
    Task<GetIntegratorRolesRepositoryResult> GetIntegratorRoleAsync(long integratorId, CancellationToken cancellationToken);
    Task AddIntegratorRoleAsync(long integratorId, string role, CancellationToken cancellationToken);
}