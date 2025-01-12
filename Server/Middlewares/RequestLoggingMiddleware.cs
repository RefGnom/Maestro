using Maestro.Server.Extensions;
using Maestro.Server.Repositories;

namespace Maestro.Server.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(nameof(RequestLoggingMiddleware));
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext, IApiKeysRepository apiKeysRepository)
    {
        var remoteIpAddress = httpContext.GetRemoteIpAddress();

        _logger.LogInformation("Handled request. Remote Ip: {remoteIp}", remoteIpAddress);

        await _next.Invoke(httpContext);

        _logger.LogInformation("Request processed. StatusCode: {statusCode}", httpContext.Response.StatusCode);
    }
}