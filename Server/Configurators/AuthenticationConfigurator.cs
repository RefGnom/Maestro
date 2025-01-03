using Maestro.Server.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Maestro.Server.Configurators;

public static class AuthenticationConfigurator
{
    public static void AddAuthenticationWithSchemes(this IServiceCollection services)
    {
        services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AuthenticationSchemes.ApiKey, null)
            .AddScheme<AuthenticationSchemeOptions, AdminApiKeyAuthenticationHandler>(AuthenticationSchemes.AdminApiKey, null);
    }
}