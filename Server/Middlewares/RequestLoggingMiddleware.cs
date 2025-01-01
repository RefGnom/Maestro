using Maestro.Core.Logging;
using Maestro.Server.Repositories;

namespace Maestro.Server.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogFactory logFactory)
{
    private readonly ILog<RequestLoggingMiddleware> _log = logFactory.CreateLog<RequestLoggingMiddleware>();
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext, IApiKeysRepository apiKeysRepository)
    {
        var remoteIp = httpContext.Request.Headers["X-Remote-Ip"].SingleOrDefault() ??
                       httpContext.Connection.RemoteIpAddress?.ToString() ?? "<Unknown>";

        _log.Info($"Handled request. Endpoint: {httpContext.Request.Path}. Remote Ip: {remoteIp}");

        await _next.Invoke(httpContext);
    }
}