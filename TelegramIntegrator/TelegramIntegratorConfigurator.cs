using Maestro.Client.Integrator;
using Maestro.Core.Configuration;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator;

public class TelegramIntegratorConfigurator : ConfiguratorBase
{
    protected override void Customize(IServiceCollection container)
    {
        container.AddSingleton<ITelegramBotClient>(
            s => new TelegramBotClient(s.GetRequiredService<ISettingsProvider>().Get("TelegramBotToken"))
        );
        container.AddSingleton<IMaestroApiClient>(
            serviceProvider =>
            {
                var maestroClientFactory = serviceProvider.GetRequiredService<IMaestroApiClientFactory>();
                var settingsProvider = serviceProvider.GetRequiredService<ISettingsProvider>();
                var uri = settingsProvider.Get("MaestroUri");
                var apiKey = settingsProvider.Get("MaestroApiKey");
                return maestroClientFactory.Create(uri, apiKey);
            }
        );
        container.AddSingleton<Lazy<IStatesProvider>>(x => new Lazy<IStatesProvider>(x.GetRequiredService<IStatesProvider>));
    }
}