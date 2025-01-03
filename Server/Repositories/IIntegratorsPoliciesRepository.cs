namespace Maestro.Server.Repositories;

public interface IIntegratorsPoliciesRepository
{
    Task<List<string>> GetIntegratorPoliciesAsync(long integratorId, CancellationToken cancellationToken);
    Task AddIntegratorPolicyAsync(long integratorId, string policy, CancellationToken cancellationToken);
}