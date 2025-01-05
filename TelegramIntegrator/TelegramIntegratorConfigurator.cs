using Maestro.Core.Configuration;
using Maestro.Core.Providers;
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
    }
}