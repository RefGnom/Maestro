﻿using Microsoft.Extensions.Configuration;

namespace Core.Providers;

public class SettingsProvider(IConfiguration settings) : ISettingsProvider
{
    public string Get(string key)
    {
        return settings[key] ?? throw new ArgumentException($"Key {key} is not found");
    }
}