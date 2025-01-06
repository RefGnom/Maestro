using System.Collections.Immutable;
using Maestro.Core.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Maestro.Server.Authentication;

public class IntegratorsRolesCache(IDateTimeProvider dateTimeProvider) : IIntegratorsRolesCache
{
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public void Set(long integratorId, List<string> roles)
    {
        if (_memoryCache.TryGetValue(integratorId, out _))
        {
            throw new ArgumentException($"Role with IntegratorId {integratorId} already exists", nameof(integratorId));
        }
        
        _memoryCache.Set(integratorId, ImmutableList.CreateRange(roles), _dateTimeProvider.GetCurrentDateTime().AddMinutes(15));
    }

    public bool TryGetRoles(long integratorId, out IImmutableList<string>? roles)
    {
        return _memoryCache.TryGetValue(integratorId, out roles);
    }
}