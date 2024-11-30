using System.Reflection;
using Maestro.Core.Configuration.ConfigurationAttributes;

namespace Maestro.Core.Configuration.DependencyInjectionConfiguration;

public static class RegistrationExtensions
{
    public static Registration GetGenericDefinitionTypeIfNeed(this Registration registration)
    {
        if (registration.InterfaceType.IsGenericType && registration.ImplementationType.IsGenericType)
        {
            return new Registration(
                registration.InterfaceType.GetGenericTypeDefinition(),
                registration.ImplementationType.GetGenericTypeDefinition(),
                registration.Lifestyle
            );
        }

        return registration;
    }

    public static ConfigurationScopeAttribute? GetScopeAttribute(this Registration registration)
    {
        return registration.ImplementationType.GetCustomAttribute<ConfigurationScopeAttribute>()
               ?? registration.InterfaceType.GetCustomAttribute<ConfigurationScopeAttribute>();
    }
}