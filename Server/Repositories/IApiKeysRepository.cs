namespace Maestro.Server.Repositories;

public interface IApiKeysRepository
{
    Task<long> GetLastIntegratorIdAsync(CancellationToken cancellationToken);
    Task<long?> GetApiKeyIntegratorIdAsync(string apiKeyHash, CancellationToken cancellationToken);
    Task<long?> AddApiKeyAsync(string apiKeyHash, long integratorId, CancellationToken cancellationToken);
}