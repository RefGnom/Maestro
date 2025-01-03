using Maestro.Data;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class IntegratorsPoliciesRepository(DataContext dataContext) : IIntegratorsPoliciesRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task AddIntegratorPolicyAsync(long integratorId, string policy, CancellationToken cancellationToken)
    {
        await _dataContext.IntegratorsPoliciesDbo.AddAsync(new IntegratorPolicyDbo { IntegratorId = integratorId, Policy = policy },
            cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetIntegratorPoliciesAsync(long integratorId, CancellationToken cancellationToken)
    {
        var integratorPolicies = await _dataContext.IntegratorsPoliciesDbo
            .Where(integratorPolicyDbo => integratorPolicyDbo.IntegratorId == integratorId)
            .Select(integratorPolicyDbo => integratorPolicyDbo.Policy)
            .ToListAsync(cancellationToken);

        return integratorPolicies;
    }
}