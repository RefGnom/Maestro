namespace Maestro.Client.Integrator;

public interface IMaestroApiClientFactory
{
    IMaestroApiClient Create(string uri, string apiKey);
}