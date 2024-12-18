using Maestro.Core.Configuration.DependencyInjectionConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Core.Configuration;

public abstract class ConfiguratorBase
{
    public void Configure(IServiceCollection container)
    {
        container.Configure().ConfigureSettings().ConfigureApplication();
        Customize(container);
    }

    protected virtual void Customize(IServiceCollection container)
    {
    }
}