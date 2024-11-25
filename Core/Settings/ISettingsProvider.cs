namespace Client;

public interface ISettingsProvider
{
    string? Get(string key);
}