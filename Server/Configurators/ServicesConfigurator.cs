using Maestro.Core.Providers;
using Maestro.Server.Authentication;
using Maestro.Server.Authorization;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authorization;

namespace Maestro.Server.Configurators;

public static class ServicesConfigurator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationHandler, ApiKeyAuthorizationHandler>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IApiKeysPoliciesCache, ApiKeysPoliciesCache>();
        services.AddSingleton<IApiKeysIntegratorsCache, ApiKeysIntegratorsCache>();
        services.AddSingleton<IApiKeyHasher, ApiKeyHasher>();
        services.AddSingleton<IPoliciesValidator, PoliciesValidator>();
    }
}