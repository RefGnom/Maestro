using Maestro.Data;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class ApiKeysRepository(DataContext dataContext) : IApiKeysRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<long?> GetApiKeyIntegratorIdAsync(string apiKeyHash, CancellationToken cancellationToken)
    {
        var apiKeyDbo =
            await _dataContext.IntegratorsApiKeys.SingleOrDefaultAsync(
                apiKeyDbo => apiKeyDbo.ApiKey == apiKeyHash && apiKeyDbo.State == ApiKeyState.Active, cancellationToken);
        return apiKeyDbo?.IntegratorId;
    }

    public async Task<long> GetLastIntegratorIdAsync(CancellationToken cancellationToken)
    {
        var lastIntegratorId = await _dataContext.IntegratorsApiKeys.MaxAsync(integratorApiKey => integratorApiKey.IntegratorId, cancellationToken);
        return lastIntegratorId;
    }

    public async Task<long?> AddApiKeyAsync(string apiKeyHash, long integratorId, CancellationToken cancellationToken)
    {
        var isApiKeyExists = await _dataContext.IntegratorsApiKeys.AnyAsync(apiKeyDbo => apiKeyDbo.ApiKey == apiKeyHash, cancellationToken);

        if (isApiKeyExists)
        {
            return null;
        }

        var addedApiKeyEntity = (await _dataContext.IntegratorsApiKeys.AddAsync(new IntegratorApiKeyDbo
            { ApiKey = apiKeyHash, IntegratorId = integratorId, State = ApiKeyState.Active }, cancellationToken)).Entity;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return addedApiKeyEntity.Id;
    }
}