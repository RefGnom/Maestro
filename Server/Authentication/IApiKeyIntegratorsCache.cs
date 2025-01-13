namespace Maestro.Server.Authentication;

public interface IApiKeyIntegratorsCache
{
    void Set(string apiKey, long integratorId);
    bool TryGetIntegratorId(string apiKey, out long? integratorId);
}