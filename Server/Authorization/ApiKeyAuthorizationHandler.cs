using System.Collections.Concurrent;
using Maestro.Server.Extensions;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Maestro.Server.Authorization;

public class ApiKeyAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor,
    IIntegratorsPoliciesRepository integratorsPoliciesRepository,
    IApiKeysPoliciesCache apiKeysPoliciesCache,
    ILoggerFactory loggerFactory)
    : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IApiKeysPoliciesCache _apiKeysPoliciesCache = apiKeysPoliciesCache;
    private readonly IIntegratorsPoliciesRepository _integratorsPoliciesRepository = integratorsPoliciesRepository;
    private readonly ILogger _logger = loggerFactory.CreateLogger(nameof(ApiKeyAuthorizationHandler));

    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        if (!context.User.Identity!.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        _logger.LogInformation("Resource with policy {policy} requested", requirement.Policy);

        var httpContext = _httpContextAccessor.HttpContext!;
        var integratorId = httpContext.GetIntegratorId();

        if (_apiKeysPoliciesCache.TryGetPolicies(integratorId, out var cachedPolicies))
        {
            if (cachedPolicies!.Contains(requirement.Policy))
            {
                _logger.LogInformation("Cached policy {policy} succeed", requirement.Policy);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Cached policy {policy} failed", requirement.Policy);
                context.Fail();
            }

            return;
        }

        var integratorPolicies = await _integratorsPoliciesRepository.GetIntegratorPoliciesAsync(integratorId, httpContext.RequestAborted);
        _apiKeysPoliciesCache.Set(integratorId, integratorPolicies);

        if (integratorPolicies.Contains(requirement.Policy))
        {
            _logger.LogInformation("Policies resolved. Count: {policiesCount}. Requested policy {policy} succeed", integratorPolicies.Count,
                requirement.Policy);
            context.Succeed(requirement);
            return;
        }

        _logger.LogInformation("Policies resolved. Count: {policiesCount}. Requested policy {policy} failed", integratorPolicies.Count,
            requirement.Policy);
        context.Fail();
    }
}