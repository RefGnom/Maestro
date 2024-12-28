using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Core.Configuration;

public static class ApplicationRunner
{
    public static async Task RunAsync<TApplication, TConfigurator>()
        where TApplication : class, IApplication
        where TConfigurator : ConfiguratorBase, new()
    {
        var container = new ServiceCollection();
        var configurator = new TConfigurator();
        configurator.Configure(container);

        var serviceProvider = container.BuildServiceProvider();
        var application = serviceProvider.GetRequiredService<TApplication>();

        application.SetUp();
        await application.RunAsync();

        var taskCompletionSource = new TaskCompletionSource();
        await taskCompletionSource.Task;
    }
}