using System.Security.Claims;
using System.Text.Encodings.Web;
using Maestro.Server.Repositories;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Maestro.Server.Authentication;

public class AdminApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    private readonly string _adminApiKey = configuration.GetValue<string>("AdminApiKey")!;

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
        if (apiKey != _adminApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Incorrect ApiKey"));
        }
        
        var authenticationTicket = CreateAuthenticationTicket();
        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    private AuthenticationTicket CreateAuthenticationTicket()
    {
        var claims = new List<Claim> { new(ClaimTypes.Role, Roles.Admin) };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        return authenticationTicket;
    }
}