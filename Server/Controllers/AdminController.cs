using Maestro.Server.Authentication;
using Maestro.Server.Private.Authentication;
using Maestro.Server.Private.Models;
using Maestro.Server.Private.Services;
using Maestro.Server.Repositories;
using Maestro.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthenticationSchemes.AdminApiKey)]
[ApiController]
[Route("admin")]
public class AdminController(
    IApiKeysRepository apiKeysRepository,
    IIntegratorsRolesRepository integratorsRolesRepository,
    IRolesValidator rolesValidator,
    IApiKeyHasher apiKeyHasher,
    ILoggerFactory logFactory) : ControllerBase
{
    private static readonly object Lock = new();

    private readonly IApiKeysRepository _apiKeysRepository = apiKeysRepository;
    private readonly IIntegratorsRolesRepository _integratorsRolesRepository = integratorsRolesRepository;
    private readonly IRolesValidator _rolesValidator = rolesValidator;
    private readonly IApiKeyHasher _apiKeyHasher = apiKeyHasher;
    private readonly ILogger _logger = logFactory.CreateLogger<AdminController>();

    [HttpPost("integrator")]
    public ActionResult Integrator([FromBody] NewIntegratorDto newIntegratorDto)
    {
        lock (Lock)
        {
            var newIntegratorId = _apiKeysRepository.GetLastIntegratorIdAsync(HttpContext.RequestAborted).Result + 1;
            var apiKeyHash = _apiKeyHasher.Hash(newIntegratorDto.ApiKey);
            var apiKeyId = _apiKeysRepository.AddApiKeyAsync(apiKeyHash, newIntegratorId, HttpContext.RequestAborted).Result;

            if (apiKeyId is null)
            {
                return new ConflictResult();
            }

            _integratorsRolesRepository.AddIntegratorRoleAsync(newIntegratorId, newIntegratorDto.Role, HttpContext.RequestAborted).Wait();
            _logger.LogInformation("New Integrator created. IntegratorId: {integratorId}. Assigned Role: {role}. Issued ApiKey: {apiKeyId}",
                newIntegratorId, newIntegratorDto.Role, apiKeyId);

            return new CreatedResult
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}