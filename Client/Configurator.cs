using Core;
using Core.IO;
using Core.Logging;
using Core.Providers;
using Data.Factories;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Client;

public static class Configurator
{
    public static Container ConfigureServices(this Container container)
    {
        container.RegisterSingleton<IWriter, Writer>();
        container.RegisterSingleton<IDateTimeProvider, DateTimeProvider>();
        container.RegisterSingleton<IGuidFactory, GuidFactory>();
        container.RegisterSingleton<IEventFactory, EventFactory>();
        container.RegisterSingleton<MaestroService>();
        container.RegisterSingleton<IEventsApiService, EventsApiService>();
        container.RegisterSingleton<ILogFactory, LogFactory>();
        container.RegisterSingleton<MaestroBotRunner>();
        container.RegisterSingleton<IMessageParser, MessagesParser>();
        return container;
    }

    public static Container ConfigureSettings(this Container container)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "../../..");
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration config = builder.Build();
        container.RegisterInstance<ISettingsProvider>(new SettingsProvider(config));
        return container;
    }
}