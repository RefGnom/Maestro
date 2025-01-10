using Maestro.Server.Repositories.Results.ApiKeys;

namespace Maestro.Server.Repositories;

public interface IApiKeysRepository
{
    Task<GetApiKeyIntegratorIdRepositoryResult> GetApiKeyIntegratorIdAsync(string apiKeyHash, CancellationToken cancellationToken);
    Task<AddApiKeyRepositoryResult> AddApiKeyAsync(string apiKeyHash, long integratorId, CancellationToken cancellationToken);
}