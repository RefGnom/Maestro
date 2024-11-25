using Core;
using Core.DateTime;
using Core.IO;
using Core.Logging;
using Data.Models;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Client;

public static class Configurator
{
    public static Container ConfigureServices(this Container container)
    {
        container.RegisterSingleton<IWriter, Writer>();
        container.RegisterSingleton<IDateTimeProvider, DateTimeProvider>();
        container.RegisterSingleton<MaestroService>();
        container.RegisterSingleton<IEventsApiService, EventsApiService>();
        container.RegisterSingleton<ILogFactory, LogFactory>();
        container.RegisterSingleton<MaestroBotRunner>();
        container.RegisterSingleton<IMessageParser<Event>, MessagesParser>();
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