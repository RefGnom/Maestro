using Maestro.Core.Logging;

namespace Maestro.Client.Integrator;

public partial class MaestroApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILog<MaestroApiClient> _log;

    private bool _isDisposed;

    public MaestroApiClient(string uri, string apiKey, ILogFactory logFactory)
    {
        if (string.IsNullOrEmpty(uri))
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentNullException(nameof(apiKey));
        }

        ArgumentNullException.ThrowIfNull(logFactory, nameof(logFactory));

        _httpClient = new HttpClient { BaseAddress = new Uri(uri), DefaultRequestHeaders = { { "Authorization", apiKey } } };
        _log = logFactory.CreateLog<MaestroApiClient>();
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _httpClient.Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}