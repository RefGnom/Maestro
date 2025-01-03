using Maestro.Server.Authentication;
using Maestro.Server.Authorization;

namespace Maestro.Server.Configurators;

public static class AuthorizationConfigurator
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.Daemon,
                policy => policy.Requirements.Add(new ApiKeyRequirement(AuthorizationPolicies.Daemon)))
            .AddPolicy(AuthorizationPolicies.Integrator,
                policy => policy.Requirements.Add(new ApiKeyRequirement(AuthorizationPolicies.Integrator)))
            .AddPolicy(AuthorizationPolicies.Admin,
                policy => policy.RequireAuthenticatedUser());
    }
}