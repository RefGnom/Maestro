using Maestro.Client.Daemon;
using Maestro.Core.Configuration;
using Maestro.Core.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Daemon.Configuration;

public class DaemonConfigurator : ConfiguratorBase
{
    protected override void Customize(IServiceCollection container)
    {
        container.AddSingleton<IDaemonMaestroApiClient>(
            serviceProvider =>
            {
                var maestroClientFactory = serviceProvider.GetRequiredService<IDaemonMaestroApiClientFactory>();
                var settingsProvider = serviceProvider.GetRequiredService<ISettingsProvider>();
                var uri = settingsProvider.Get("MaestroDaemonUri");
                var apiKey = settingsProvider.Get("MaestroDaemonApiKey");
                return maestroClientFactory.Create(uri, apiKey);
            }
        );
    }
}