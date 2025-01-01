namespace Maestro.Server.Middlewares.Extensions;

public static class ApiKeysAuthorizationMiddlewareExtensions
{
    public static void UseApiKeysAuthorization(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApiKeysAuthorizationMiddleware>();
    }
}