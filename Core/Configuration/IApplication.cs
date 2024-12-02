using Maestro.Core.Configuration.ConfigurationAttributes;

namespace Maestro.Core.Configuration;

[ConfigurationIgnore]
public interface IApplication
{
    void SetUp();
    Task RunAsync();
}