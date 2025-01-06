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
    IIntegratorsRolesCache integratorsRolesCache,
    IIntegratorsRolesRepository integratorsRolesRepository,
    IApiKeysRepository apiKeysRepository,
    IApiKeyHasher apiKeyHasher)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ApiKeyAuthenticationHandler>();
    private readonly IApiKeysIntegratorsCache _apiKeysIntegratorsCache = apiKeysIntegratorsCache;
    private readonly IIntegratorsRolesCache _integratorsRolesCache = integratorsRolesCache;
    private readonly IIntegratorsRolesRepository _integratorsRolesRepository = integratorsRolesRepository;
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

        if (_apiKeysIntegratorsCache.TryGetIntegratorId(apiKey, out var cachedIntegratorId))
        {
            _logger.LogInformation("ApiKey resolved. Cached IntegratorId: {integratorId}", cachedIntegratorId);
            authenticationTicket = await CreateAuthenticationTicketAsync(cachedIntegratorId!.Value);
            return AuthenticateResult.Success(authenticationTicket);
        }

        var integratorId = await _apiKeysRepository.GetApiKeyIntegratorIdAsync(apiKeyHash, Context.RequestAborted);

        if (integratorId is null)
        {
            return AuthenticateResult.Fail("Unable to resolve ApiKey");
        }

        _logger.LogInformation("ApiKey resolved. IntegratorId: {integratorId}", integratorId);
        _apiKeysIntegratorsCache.Set(apiKey, integratorId.Value);
        authenticationTicket = await CreateAuthenticationTicketAsync(integratorId.Value);

        return AuthenticateResult.Success(authenticationTicket);
    }

    private static void AddIntegratorRoles(List<Claim> claims, IEnumerable<string> roles)
    {
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    }

    private async Task<AuthenticationTicket> CreateAuthenticationTicketAsync(long integratorId)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, integratorId.ToString()) };
        await AddIntegratorRolesAsync(claims, integratorId);

        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        return authenticationTicket;
    }

    private async Task AddIntegratorRolesAsync(List<Claim> claims, long integratorId)
    {
        if (_integratorsRolesCache.TryGetRoles(integratorId, out var cachedRoles))
        {
            _logger.LogInformation("Roles resolved. Cached Roles: {roles}", string.Join(", ", cachedRoles!));
            AddIntegratorRoles(claims, cachedRoles!);
        }
        else
        {
            var integratorRoles = await _integratorsRolesRepository.GetIntegratorRolesAsync(integratorId, Context.RequestAborted);
            _integratorsRolesCache.Set(integratorId, integratorRoles);
            _logger.LogInformation("Roles resolved. Roles: {roles}", string.Join(", ", integratorRoles));
            AddIntegratorRoles(claims, integratorRoles);
        }
    }
}