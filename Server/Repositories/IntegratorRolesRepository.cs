using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Repositories.Results.IntegratorRoles;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class IntegratorRolesRepository(DataContext dataContext) : IIntegratorRolesRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task AddIntegratorRoleAsync(long integratorId, string role, CancellationToken cancellationToken)
    {
        await _dataContext.IntegratorsRolesDbo.AddAsync(new IntegratorRoleDbo { IntegratorId = integratorId, Role = role }, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetIntegratorRolesRepositoryResult> GetIntegratorRoleAsync(long integratorId, CancellationToken cancellationToken)
    {
        var integratorRole = await _dataContext.IntegratorsRolesDbo
            .Where(integratorRoleDbo => integratorRoleDbo.IntegratorId == integratorId)
            .Select(integratorRoleDbo => integratorRoleDbo.Role)
            .SingleOrDefaultAsync(cancellationToken);

        return integratorRole is null
            ? new GetIntegratorRolesRepositoryResult(false, null) { IsIntegratorRoleFound = false }
            : new GetIntegratorRolesRepositoryResult(true, integratorRole);
    }
}