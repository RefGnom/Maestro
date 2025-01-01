namespace Maestro.Server.Repositories;

public interface IApiKeysRepository
{
    Task<long?> GetIntegratorIdAsync(string apiKey, CancellationToken cancellationToken);
}