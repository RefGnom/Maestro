using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;

namespace Maestro.Server.Configurators;

public static class ServicesConfigurator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IWriter, Writer>();
        services.AddSingleton<ILogFactory, LogFactory>();
    }
}