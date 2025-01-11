using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Maestro.Server.Authentication;

public abstract class ServiceApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder,
    IConfiguration configuration) : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    private readonly IConfiguration _configuration = configuration;

    private string? _apiKey;

    private string ApiKey
    {
        get
        {
            if (_apiKey is not null)
            {
                return _apiKey;
            }

            _apiKey = _configuration.GetValue<string>(ApiKeyConfigurationName);
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentException(nameof(_apiKey));
            }

            return _apiKey;
        }
    }

    protected abstract string ApiKeyConfigurationName { get; }
    protected abstract string ProvideRole { get; }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKeyHeaderValues = Request.Headers.Authorization;
        switch (apiKeyHeaderValues.Count)
        {
            case 0:
                return Task.FromResult(AuthenticateResult.Fail("No Authorization header"));
            case > 1:
                return Task.FromResult(AuthenticateResult.Fail("Too many values of Authorization header"));
        }

        var apiKey = apiKeyHeaderValues.Single();
        if (apiKey != ApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Incorrect ApiKey"));
        }

        var authenticationTicket = CreateAuthenticationTicket();
        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    private AuthenticationTicket CreateAuthenticationTicket()
    {
        var claims = new List<Claim> { new(ClaimTypes.Role, ProvideRole) };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        return authenticationTicket;
    }
}