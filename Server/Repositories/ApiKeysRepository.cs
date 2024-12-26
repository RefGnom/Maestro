using Maestro.Data;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Repositories;

public class ApiKeysRepository(DataContext dataContext) : IApiKeysRepository
{
    public async Task<long?> GetIntegratorIdAsync(string apiKey, CancellationToken cancellationToken)
    {
        var apiKeyDbo = await dataContext.ApiKeys.FirstOrDefaultAsync(apiKeyDbo => apiKeyDbo.Key == apiKey, cancellationToken: cancellationToken);
        var integratorId = apiKeyDbo?.IntegratorId ?? null;
        return integratorId;
    }
}