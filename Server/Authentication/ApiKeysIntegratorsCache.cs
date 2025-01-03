using Maestro.Core.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Maestro.Server.Authentication;

public class ApiKeysIntegratorsCache(IDateTimeProvider dateTimeProvider) : IApiKeysIntegratorsCache
{
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public void Set(string apiKey, long integratorId)
    {
        _memoryCache.Set(apiKey, integratorId, _dateTimeProvider.GetCurrentDateTime().AddMinutes(15));
    }
    
    public bool TryGetPolicies(string apiKey, out long? integratorId)
    {
        return _memoryCache.TryGetValue(apiKey, out integratorId);
    }
}