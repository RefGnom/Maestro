using Microsoft.Extensions.Configuration;

namespace Maestro.Core.Providers;

public class SettingsProvider(IConfiguration settings) : ISettingsProvider
{
    private readonly IConfiguration _settings = settings;

    public string Get(string key)
    {
        return _settings[key] ?? throw new ArgumentException($"Key {key} is not found");
    }
}