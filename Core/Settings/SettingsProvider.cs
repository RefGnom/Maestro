using Client;
using Microsoft.Extensions.Configuration;

namespace Core;

public class SettingsProvider: ISettingsProvider
{
    private readonly IConfiguration _settings;

    public SettingsProvider(IConfiguration settings)
    {
        _settings = settings;
    }

    public string Get(string key)
    {
        return _settings[key] ?? throw  new ArgumentException($"Key {key} is not found");
    }
    
    public string? SafeGet(string key)
    {
        return _settings[key];
    }
}