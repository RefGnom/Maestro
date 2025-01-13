using Maestro.Core.Providers;
using Maestro.Server.Authentication;
using Maestro.Server.Private.Services;
using Maestro.Server.Services;

namespace Maestro.Server.Configurators;

public static class ServicesConfigurator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IIntegratorsRolesCache, IntegratorsRolesCache>();
        services.AddSingleton<IApiKeyIntegratorsCache, ApiKeyIntegratorsCache>();
        services.AddSingleton<IApiKeyHasher, ApiKeyHasher>();
        services.AddSingleton<IRolesValidator, RolesValidator>();
    }
}