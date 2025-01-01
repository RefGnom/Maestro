namespace Maestro.Client;

public interface IMaestroApiClientFactory
{
    IMaestroApiClient Create(string uri, string apiKey);
}