using Maestro.Server.Repositories;

namespace Maestro.Server.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(nameof(RequestLoggingMiddleware));
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext, IApiKeysRepository apiKeysRepository)
    {
        var remoteIp = httpContext.Request.Headers["X-Remote-Ip"].SingleOrDefault() ??
                       httpContext.Connection.RemoteIpAddress?.ToString() ?? "<Unknown>";

        _logger.LogInformation("Handled request. Endpoint: {endpoint}. Remote Ip: {remoteIp}", httpContext.Request.Path, remoteIp);

        await _next.Invoke(httpContext);
    }
}