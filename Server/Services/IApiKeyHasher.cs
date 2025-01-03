namespace Maestro.Server.Services;

public interface IApiKeyHasher
{
    string Hash(string apiKey);
}