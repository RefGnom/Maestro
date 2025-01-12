using Maestro.Core.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Maestro.Server.Authentication;

public class IntegratorsRolesCache(IDateTimeProvider dateTimeProvider) : IIntegratorsRolesCache
{
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public void Set(long integratorId, string role)
    {
        if (_memoryCache.TryGetValue(integratorId, out _))
        {
            throw new ArgumentException($"Role with IntegratorId {integratorId} already exists", nameof(integratorId));
        }

        _memoryCache.Set(integratorId, role, _dateTimeProvider.GetCurrentDateTime().AddMinutes(15));
    }

    public bool TryGetRole(long integratorId, out string? role)
    {
        return _memoryCache.TryGetValue(integratorId, out role);
    }
}