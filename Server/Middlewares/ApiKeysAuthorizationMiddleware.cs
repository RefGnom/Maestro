using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Repositories;

namespace Maestro.Server.Middlewares;

public class ApiKeysAuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private readonly ILog<ApiKeysAuthorizationMiddleware> _log = new Log<ApiKeysAuthorizationMiddleware>(new DateTimeProvider(), new Writer()); // fuck cyrus

    public async Task InvokeAsync(HttpContext context, IApiKeysRepository apiKeysRepository)
    {
        var apiKeyHeaderValues = context.Request.Headers.Authorization;

        if (apiKeyHeaderValues.Count is not 1)
        {
            _log.Info("Handled authorized request. Bad Authorization header");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var apiKey = apiKeyHeaderValues.First()!;
        var integratorId = await apiKeysRepository.GetIntegratorIdAsync(apiKey, context.RequestAborted);

        if (integratorId is null)
        {
            _log.Info("Handled authorized request. ApiKey was not resolved");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        _log.Info($"Handled authorized request. IntegratorId:{integratorId}");
        await _next(context);
    }
}