using Maestro.Server.Repositories;

namespace Maestro.Server.Startup;

public static class RepositoriesConfigurator
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRemindersRepository, RemindersRepository>();
        services.AddScoped<IApiKeysRepository, ApiKeysRepository>();
    }
}