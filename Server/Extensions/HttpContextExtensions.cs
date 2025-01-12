using System.Security.Claims;

namespace Maestro.Server.Extensions;

public static class HttpContextExtensions
{
    public static long GetIntegratorId(this HttpContext httpContext)
    {
        var claim = httpContext.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier);
        var integratorId = long.Parse(claim.Value);
        return integratorId;
    }

    public static string GetRemoteIpAddress(this HttpContext httpContext)
    {
        return httpContext.Request.Headers["X-Remote-Ip"].SingleOrDefault() ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "<Unknown>";
    }

    public static string GetUserRole(this HttpContext httpContext)
    {
        return httpContext.User.Claims.Single(claim => claim.Type == ClaimTypes.Role).Value;
    }
}