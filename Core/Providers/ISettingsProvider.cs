using Maestro.Core.Configuration.ConfigurationAttributes;

namespace Maestro.Core.Providers;

[ConfigurationIgnore]
public interface ISettingsProvider
{
    string Get(string key);
}