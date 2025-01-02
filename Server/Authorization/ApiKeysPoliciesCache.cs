using System.Collections.Concurrent;
using System.Collections.Immutable;
using Maestro.Core.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Maestro.Server.Authorization;

public class ApiKeysPoliciesCache(IDateTimeProvider dateTimeProvider) : IApiKeysPoliciesCache
{
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public void Set(long integratorId, List<string> policies)
    {
        if (_memoryCache.TryGetValue(integratorId, out _))
        {
            throw new ArgumentException($"Policies with id {integratorId} already exists", nameof(integratorId));
        }
        
        _memoryCache.Set(integratorId, ImmutableList.CreateRange(policies), _dateTimeProvider.GetCurrentDateTime().AddMinutes(15));
    }

    public bool TryGetPolicies(long integratorId, out IImmutableList<string>? entry)
    {
        return _memoryCache.TryGetValue(integratorId, out entry);
    }
}