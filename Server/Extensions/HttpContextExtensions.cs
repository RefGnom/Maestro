using System.Security.Claims;

namespace Maestro.Server.Extensions;

public static class HttpContextExtensions
{
    public static long GetIntegratorId(this HttpContext httpContext)
    {
        var claim = httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)!;
        var integratorId = long.Parse(claim.Value);
        return integratorId;
    }
}