using System.Security.Claims;
using System.Text.Encodings.Web;
using Maestro.Server.Repositories;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Maestro.Server.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder,
    IApiKeysIntegratorsCache apiKeysIntegratorsCache,
    IApiKeysRepository apiKeysRepository,
    IApiKeyHasher apiKeyHasher)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(nameof(ApiKeyAuthenticationHandler));
    private readonly IApiKeysIntegratorsCache _apiKeysIntegratorsCache = apiKeysIntegratorsCache;
    private readonly IApiKeysRepository _apiKeysRepository = apiKeysRepository;
    private readonly IApiKeyHasher _apiKeyHasher = apiKeyHasher;

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKeyHeaderValues = Request.Headers.Authorization;
        switch (apiKeyHeaderValues.Count)
        {
            case 0:
                return AuthenticateResult.Fail("No Authorization header");
            case > 1:
                return AuthenticateResult.Fail("Too many values of Authorization header");
        }

        AuthenticationTicket authenticationTicket;
        var apiKey = apiKeyHeaderValues.Single()!;
        var apiKeyHash = _apiKeyHasher.Hash(apiKey);

        if (_apiKeysIntegratorsCache.TryGetPolicies(apiKey, out var cachedIntegratorId))
        {
            _logger.LogInformation("ApiKey resolved. Cached IntegratorId: {integratorId}", cachedIntegratorId);
            authenticationTicket = CreateAuthenticationTicket(cachedIntegratorId!.Value);
            return AuthenticateResult.Success(authenticationTicket);
        }

        var integratorId = await _apiKeysRepository.GetApiKeyIntegratorIdAsync(apiKeyHash, Context.RequestAborted);

        if (integratorId is null)
        {
            return AuthenticateResult.Fail("Unable to resolve ApiKey");
        }

        _logger.LogInformation("ApiKey resolved. IntegratorId: {integratorId}", integratorId);
        _apiKeysIntegratorsCache.Set(apiKey, integratorId.Value);
        authenticationTicket = CreateAuthenticationTicket(integratorId.Value);

        return AuthenticateResult.Success(authenticationTicket);

        // ReSharper disable once VariableHidesOuterVariable
        AuthenticationTicket CreateAuthenticationTicket(long integratorId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, integratorId.ToString()) };
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            // ReSharper disable once VariableHidesOuterVariable
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return authenticationTicket;
        }
    }
}