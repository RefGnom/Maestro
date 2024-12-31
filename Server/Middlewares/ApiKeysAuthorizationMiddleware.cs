using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Extensions;
using Maestro.Server.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Maestro.Server.Middlewares;

public class ApiKeysAuthorizationMiddleware(RequestDelegate next, ILogFactory logFactory, IDateTimeProvider dateTimeProvider)
{
    private readonly RequestDelegate _next = next;
    private readonly ILog<ApiKeysAuthorizationMiddleware> _log = logFactory.CreateLog<ApiKeysAuthorizationMiddleware>();
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());

    public async Task InvokeAsync(HttpContext httpContext, IApiKeysRepository apiKeysRepository)
    {
        var apiKeyHeaderValues = httpContext.Request.Headers.Authorization;

        if (apiKeyHeaderValues.Count is 0)
        {
            _log.Info("Handled unauthorized request");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (apiKeyHeaderValues.Count is not 1)
        {
            _log.Info("Handled request. Too many values of Authorization header");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var apiKey = apiKeyHeaderValues.First()!;
        var isCacheResolved = _memoryCache.TryGetValue<long>(apiKey, out var cachedIntegratorId);

        if (isCacheResolved)
        {
            _log.Info($"Handled authorized request. Cached IntegratorId: {cachedIntegratorId}");
            httpContext.SetIntegratorId(cachedIntegratorId);
            await _next(httpContext);
            return;
        }

        var integratorId = await apiKeysRepository.GetIntegratorIdAsync(apiKey, httpContext.RequestAborted);
        if (integratorId is null)
        {
            _log.Info("Handled request. ApiKey was not resolved");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        _log.Info($"Handled authorized request. IntegratorId: {integratorId}");

        httpContext.SetIntegratorId(integratorId.Value);
        _memoryCache.Set(apiKey, integratorId, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = _dateTimeProvider.GetCurrentDateTime().AddMinutes(5)
        });

        await _next(httpContext);
    }
}