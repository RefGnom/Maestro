using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Repositories.Results.ApiKeys;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class ApiKeysRepository(DataContext dataContext) : IApiKeysRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<GetApiKeyIntegratorIdRepositoryResult> GetApiKeyIntegratorIdAsync(string apiKeyHash, CancellationToken cancellationToken)
    {
        var apiKeyDbo =
            await _dataContext.IntegratorsApiKeys.SingleOrDefaultAsync(
                apiKeyDbo => apiKeyDbo.ApiKey == apiKeyHash && apiKeyDbo.State == ApiKeyState.Active, cancellationToken);

        return apiKeyDbo is null
            ? new GetApiKeyIntegratorIdRepositoryResult(false, null) { IsApiKeyFound = false }
            : new GetApiKeyIntegratorIdRepositoryResult(true, apiKeyDbo.IntegratorId);
    }

    public async Task<AddApiKeyRepositoryResult> AddApiKeyAsync(string apiKeyHash, long integratorId, CancellationToken cancellationToken)
    {
        var addedApiKeyEntity = (await _dataContext.IntegratorsApiKeys.AddAsync(new IntegratorApiKeyDbo
            { ApiKey = apiKeyHash, IntegratorId = integratorId, State = ApiKeyState.Active }, cancellationToken)).Entity;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new AddApiKeyRepositoryResult(true, addedApiKeyEntity.Id);
    }
}