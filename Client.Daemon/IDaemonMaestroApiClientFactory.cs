namespace Maestro.Client.Daemon;

public interface IDaemonMaestroApiClientFactory
{
    IDaemonMaestroApiClient Create(string uri, string apiKey);
}