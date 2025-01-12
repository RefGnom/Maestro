using Maestro.Server.Repositories;

namespace Maestro.Server.Configurators;

public static class RepositoriesConfigurator
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRemindersRepository, RemindersRepository>();
        services.AddScoped<IApiKeysRepository, ApiKeysRepository>();
        services.AddScoped<IIntegratorsRepository, IntegratorsRepository>();
        services.AddScoped<IIntegratorsRolesRepository, IntegratorsRolesRepository>();
    }
}