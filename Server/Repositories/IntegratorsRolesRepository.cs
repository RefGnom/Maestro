using Maestro.Data;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class IntegratorsRolesRepository(DataContext dataContext) : IIntegratorsRolesRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task AddIntegratorRoleAsync(long integratorId, string role, CancellationToken cancellationToken)
    {
        await _dataContext.IntegratorsRolesDbo.AddAsync(new IntegratorRoleDbo { IntegratorId = integratorId, Role = role },
            cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetIntegratorRolesAsync(long integratorId, CancellationToken cancellationToken)
    {
        var integratorRoles = await _dataContext.IntegratorsRolesDbo
            .Where(integratorRoleDbo => integratorRoleDbo.IntegratorId == integratorId)
            .Select(integratorRoleDbo => integratorRoleDbo.Role)
            .ToListAsync(cancellationToken);

        return integratorRoles;
    }
}