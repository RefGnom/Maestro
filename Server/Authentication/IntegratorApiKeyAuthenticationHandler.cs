using System.Security.Claims;
using System.Text.Encodings.Web;
using Maestro.Server.Helpers;
using Maestro.Server.Repositories;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Maestro.Server.Authentication;

public class IntegratorApiKeyAuthenticationHandler(
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
    private readonly ILogger _logger = loggerFactory.CreateLogger<IntegratorApiKeyAuthenticationHandler>();
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

        var repositoryResult = await _apiKeysRepository.GetApiKeyIntegratorIdAsync(apiKeyHash, Context.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            if (repositoryResult.IsApiKeyFound is false)
            {
                return AuthenticateResult.Fail("Unable to resolve ApiKey");
            }

            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("ApiKey resolved. IntegratorId: {integratorId}", repositoryResult);
        _apiKeysIntegratorsCache.Set(apiKey, repositoryResult.Data!.Value);
        authenticationTicket = await CreateAuthenticationTicketAsync(repositoryResult.Data!.Value);

        return AuthenticateResult.Success(authenticationTicket);
    }

    private async Task<AuthenticationTicket> CreateAuthenticationTicketAsync(long integratorId)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, integratorId.ToString()) };
        await AddIntegratorRoleAsync(claims, integratorId);

        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        return authenticationTicket;
    }

    private async Task AddIntegratorRoleAsync(List<Claim> claims, long integratorId)
    {
        if (_integratorsRolesCache.TryGetRole(integratorId, out var cachedRole))
        {
            _logger.LogInformation("Role resolved. Cached Role: {role}", cachedRole);
            claims.Add(new Claim(ClaimTypes.Role, cachedRole!));
        }
        else
        {
            var repositoryResult = await _integratorsRolesRepository.GetIntegratorRoleAsync(integratorId, Context.RequestAborted);

            if (!repositoryResult.IsSuccessful)
            {
                RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
            }

            _integratorsRolesCache.Set(integratorId, repositoryResult.Data!);
            _logger.LogInformation("Role resolved. Role: {role}", repositoryResult.Data!);
            claims.Add(new Claim(ClaimTypes.Role, repositoryResult.Data!));
        }
    }
}