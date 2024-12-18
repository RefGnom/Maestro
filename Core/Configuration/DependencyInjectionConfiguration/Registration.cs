using System.Reflection;
using Maestro.Core.Configuration.ConfigurationAttributes;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Core.Configuration.DependencyInjectionConfiguration;

public record Registration(Type InterfaceType, Type ImplementationType, ServiceLifetime Lifestyle)
{
    public bool Resolve()
    {
        var ignoreAttribute = InterfaceType.GetCustomAttribute<ConfigurationIgnoreAttribute>()
                              ?? ImplementationType.GetCustomAttribute<ConfigurationIgnoreAttribute>();

        return ignoreAttribute is null;
    }
}