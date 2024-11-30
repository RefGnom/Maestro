namespace Maestro.Core.Providers;

public interface ISettingsProvider
{
    string Get(string key);
}