using System.Reflection;
using Maestro.Client.Integrator;
using Maestro.Core.Configuration.DependencyInjectionConfiguration;
using Maestro.TelegramIntegrator;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Maestro.TelegramIntegratorTests;

public class TestBase
{
    protected readonly Assembly ServiceAssembly = Assembly.GetAssembly(typeof(TelegramIntegratorApplication))!;
    protected ServiceProvider ServiceProvider { get; private set; }

    [OneTimeSetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseAutoConfiguration();
        serviceCollection.AddSingleton<ITelegramBotClient>(new TelegramBotClient("3534534525:fghdfhfghwstrgh"));
        serviceCollection.AddSingleton<IMaestroApiClient>(
            serviceProvider =>
            {
                var maestroClientFactory = serviceProvider.GetRequiredService<IMaestroApiClientFactory>();
                return maestroClientFactory.Create("https://uri", "apiKey");
            }
        ).AddSingleton<Lazy<IStatesProvider>>(x => new Lazy<IStatesProvider>(x.GetRequiredService<IStatesProvider>));
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceProvider.Dispose();
    }
}