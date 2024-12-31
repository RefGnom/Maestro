using Maestro.Core.Logging;

namespace Maestro.Client;

public class MaestroApiClientFactory(ILogFactory logFactory) : IMaestroApiClientFactory
{
    private readonly ILogFactory _logFactory = logFactory;

    public IMaestroApiClient Create(string uri, string apiKey)
    {
        return new MaestroApiClient(uri, apiKey, _logFactory);
    }
}