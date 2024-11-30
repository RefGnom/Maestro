using SimpleInjector;

namespace Maestro.Core.Configuration.ConfigurationAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ConfigurationScopeAttribute(Lifestyle lifestyle) : Attribute
{
    public Lifestyle Lifestyle { get; } = lifestyle;
}