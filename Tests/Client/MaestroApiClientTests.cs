using Maestro.Client.Integrator;
using Maestro.Core.IO;
using Maestro.Core.Logging;
using Maestro.Core.Providers;

namespace Maestro.Tests.Client;

[TestFixture]
public partial class MaestroApiClientTests
{
    private MaestroApiClient _maestroApiClient;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        const string uri = "http://localhost:5000/api/v1/";
        const string apiKey = "integrator123";

        _maestroApiClient = new MaestroApiClient(uri, apiKey,
            // Substitute.For<ILogFactory>()
            new LogFactory(new DateTimeProvider(), new Writer())
        );
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _maestroApiClient.Dispose();
    }
}