using System.Text.Encodings.Web;
using Maestro.Server.Private.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Maestro.Server.Authentication;

public class AdminApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder,
    IConfiguration configuration) : ServiceApiKeyAuthenticationHandler(options, loggerFactory, encoder, configuration)
{
    protected override string ApiKeyConfigurationName => "AdminApiKey";
    protected override string ProvideRole => ServiceRoles.Admin;
}