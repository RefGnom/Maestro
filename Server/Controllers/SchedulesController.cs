using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Authentication;
using Maestro.Server.Extensions;
using Maestro.Server.Helpers;
using Maestro.Server.Private.Authentication;
using Maestro.Server.Public.Models.Schedules;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[Authorize(Roles = IntegratorsRoles.Base, AuthenticationSchemes = AuthenticationSchemes.IntegratorApiKey)]
[ApiController]
[Route("api/v1/schedules")]
public class SchedulesController(ISchedulesRepository schedulesRepository, IMapper mapper, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<SchedulesController>();
    private readonly IMapper _mapper = mapper;
    private readonly ISchedulesRepository _schedulesRepository = schedulesRepository;

    #region Get

    [HttpGet("all")]
    public async Task<ActionResult> All([FromBody] AllSchedulesDto allSchedulesDto)
    {
        var repositoryResult =
            await _schedulesRepository.GetAllSchedulesAsync(allSchedulesDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Fetched SchedulesCount: {schedulesCount}", repositoryResult.Data!.Count);

        var scheduleWithIdDtos = _mapper.Map<List<ScheduleWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(scheduleWithIdDtos);
    }
    
    [HttpGet("byId")]
    public async Task<ActionResult> ById([FromBody] ScheduleIdDto scheduleIdDto)
    {
        var repositoryResult =
            await _schedulesRepository.GetByIdAsync(scheduleIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            if (repositoryResult.IsScheduleFound is false)
            {
                _logger.LogWarning("Operation failed. Schedule was not found. ScheduleId: {scheduleId}", scheduleIdDto.ScheduleId);
                return new NotFoundResult();
            }

            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        var scheduleDto = _mapper.Map<ScheduleDto>(repositoryResult.Data);

        _logger.LogInformation("Fetched ScheduleId: {scheduleId}", repositoryResult.Data!.Id);

        return new OkObjectResult(scheduleDto);
    }
    
    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] SchedulesForUserDto schedulesForUserDto)
    {
        var repositoryResult =
            await _schedulesRepository.GetForUserAsync(schedulesForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Fetched SchedulesCount: {schedulesCount}", repositoryResult.Data!.Count);

        var scheduleWithIdDtos = _mapper.Map<List<ScheduleWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(scheduleWithIdDtos);
    }

    #endregion

    #region Post

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ScheduleDto scheduleDto)
    {
        var scheduleDbo = _mapper.Map<ScheduleDbo>(scheduleDto);
        scheduleDbo.IntegratorId = HttpContext.GetIntegratorId();

        var repositoryResult = await _schedulesRepository.AddAsync(scheduleDbo, HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            if (repositoryResult.IsScheduleOverlap is true)
            {
                var scheduleIdDtos = repositoryResult.OverlapScheduleIds!
                    .Select(scheduleId => new ScheduleIdDto { ScheduleId = scheduleId })
                    .ToList();

                _logger.LogWarning("Operation failed. Schedule overlapped. OverlappedScheduleIds: {overlappedScheduleIds}",
                    string.Join(",", repositoryResult.OverlapScheduleIds!));

                return new ConflictObjectResult(scheduleIdDtos);
            }

            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Created ScheduleId: {scheduleId}", repositoryResult.Data);

        return new ObjectResult(new ScheduleIdDto { ScheduleId = repositoryResult.Data!.Value })
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    #endregion

    #region Patch

    [HttpPatch("setCompleted")]
    public async Task<ActionResult> SetCompleted([FromBody] ScheduleIdDto scheduleIdDto)
    {
        var repositoryResult =
            await _schedulesRepository.SetScheduleCompleted(scheduleIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful) return new OkResult();

        if (repositoryResult.IsScheduleFound is false)
        {
            _logger.LogWarning("Operation failed. ScheduleId was not found. ScheduleId: {scheduleId}", scheduleIdDto.ScheduleId);
            return new NotFoundResult();
        }

        if (repositoryResult.IsCompletedAlreadySet is not true)
        {
            throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
        }

        _logger.LogWarning("Operation failed. Completed already set");
        return new ConflictResult();
    }

    #endregion
}