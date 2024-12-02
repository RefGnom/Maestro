using System.Reflection;
using Maestro.Core.Configuration.ConfigurationAttributes;
using SimpleInjector;

namespace Maestro.Core.Configuration.DependencyInjectionConfiguration;

public record Registration(Type InterfaceType, Type ImplementationType, Lifestyle Lifestyle)
{
    public bool Resolve()
    {
        var ignoreAttribute = InterfaceType.GetCustomAttribute<ConfigurationIgnoreAttribute>()
                              ?? ImplementationType.GetCustomAttribute<ConfigurationIgnoreAttribute>();

        return ignoreAttribute is null;
    }
}