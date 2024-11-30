using Maestro.Core.Configuration.DependencyInjectionConfiguration;
using SimpleInjector;

namespace Maestro.Core.Configuration;

public abstract class ConfiguratorBase
{
    public void Configure(Container container)
    {
        container.Configure().ConfigureSettings().ConfigureApplication();
        Customize(container);
    }

    protected virtual void Customize(Container container)
    {
    }
}