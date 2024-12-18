using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Core.Configuration.ConfigurationAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ConfigurationScopeAttribute(ServiceLifetime lifestyle) : Attribute
{
    public ServiceLifetime Lifestyle { get; } = lifestyle;
}