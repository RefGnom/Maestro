using Maestro.Core.Logging;

namespace Maestro.Client.Daemon;

public class DaemonMaestroApiClientFactory(ILogFactory logFactory) : IDaemonMaestroApiClientFactory
{
    private readonly ILogFactory _logFactory = logFactory;

    public IDaemonMaestroApiClient Create(string uri, string apiKey)
    {
        return new DaemonMaestroApiClient(uri, apiKey, _logFactory);
    }
}