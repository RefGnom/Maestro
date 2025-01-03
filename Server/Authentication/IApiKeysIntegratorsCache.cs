namespace Maestro.Server.Authentication;

public interface IApiKeysIntegratorsCache
{
    void Set(string apiKey, long integratorId);
    bool TryGetPolicies(string apiKey, out long? integratorId);
}