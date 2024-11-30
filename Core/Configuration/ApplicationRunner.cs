using SimpleInjector;

namespace Maestro.Core.Configuration;

public static class ApplicationRunner
{
    public static async Task RunAsync<TApplication, TConfigurator>()
    where TApplication : class, IApplication
    where TConfigurator : ConfiguratorBase, new()
    {
        var container = new Container();
        var configurator = new TConfigurator();
        configurator.Configure(container);

        var application = container.GetInstance<TApplication>();
        application.SetUp();
        await application.RunAsync();
    }
}