using Maestro.Server.Authentication;
using Maestro.Server.Authorization;
using Maestro.Server.Core.Models;
using Maestro.Server.Repositories;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[Authorize(Policy = AuthorizationPolicies.Admin, AuthenticationSchemes = AuthenticationSchemes.AdminApiKey)]
[ApiController]
[Route("admin")]
public class AdminController(
    IApiKeysRepository apiKeysRepository,
    IIntegratorsPoliciesRepository integratorsPoliciesRepository,
    IPoliciesValidator policiesValidator,
    IApiKeyHasher apiKeyHasher,
    ILoggerFactory logFactory) : ControllerBase
{
    private static readonly object Lock = new();

    private readonly IApiKeysRepository _apiKeysRepository = apiKeysRepository;
    private readonly IIntegratorsPoliciesRepository _integratorsPoliciesRepository = integratorsPoliciesRepository;
    private readonly IPoliciesValidator _policiesValidator = policiesValidator;
    private readonly IApiKeyHasher _apiKeyHasher = apiKeyHasher;
    private readonly ILogger _logger = logFactory.CreateLogger(nameof(AdminController));

    [HttpPost("integrator")]
    public async Task<ActionResult> Integrator([FromBody] NewIntegratorDto newIntegratorDto)
    {
        try
        {
            Monitor.Enter(Lock);
            
            var newIntegratorId = await _apiKeysRepository.GetLastIntegratorIdAsync(HttpContext.RequestAborted) + 1;
            var apiKeyHash = _apiKeyHasher.Hash(newIntegratorDto.ApiKey);
            var apiKeyId = await _apiKeysRepository.AddApiKey(apiKeyHash, newIntegratorId, HttpContext.RequestAborted);

            if (apiKeyId is null)
            {
                return new ConflictResult();
            }

            if (!_policiesValidator.Validate(newIntegratorDto.Policy))
            {
                return new BadRequestResult();
            }

            await _integratorsPoliciesRepository.AddIntegratorPolicyAsync(newIntegratorId, newIntegratorDto.Policy,
                HttpContext.RequestAborted);
            _logger.LogInformation("Policy {policy} issued to new Integrator. IntegratorId: {integratorId}", newIntegratorDto.Policy, newIntegratorId);

            return new CreatedResult();
        }
        finally
        {
            Monitor.Exit(Lock);
        }
    }
}