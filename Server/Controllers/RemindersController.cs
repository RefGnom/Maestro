using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Extensions;
using Maestro.Server.Helpers;
using Maestro.Server.Private.Authentication;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationSchemes = Maestro.Server.Authentication.AuthenticationSchemes;

namespace Maestro.Server.Controllers;

[Authorize(Roles = Roles.Integrator, AuthenticationSchemes = AuthenticationSchemes.ApiKey)]
[ApiController]
[Route("api/v1/reminders")]
public class RemindersController(IRemindersRepository remindersRepository, IMapper mapper, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<RemindersController>();
    private readonly IMapper _mapper = mapper;
    private readonly IRemindersRepository _remindersRepository = remindersRepository;

    #region Get

    [HttpGet("all")]
    public async Task<ActionResult> All([FromBody] AllRemindersDto allRemindersDto)
    {
        var repositoryResult =
            await _remindersRepository.GetAllRemindersAsync(allRemindersDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        var reminderWithIdDtos = _mapper.Map<List<ReminderWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(reminderWithIdDtos);
    }

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var repositoryResult =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Fetched RemindersCount: {remindersCount}", repositoryResult.Data!.Count);

        var reminderWithIdsDtos = _mapper.Map<List<ReminderWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(reminderWithIdsDtos);
    }

    [HttpGet("byId")]
    public async Task<ActionResult> ById([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult =
            await _remindersRepository.GetByIdAsync(reminderIdDto.ReminderId, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            if (repositoryResult.IsReminderFound is false)
            {
                return new NotFoundResult();
            }

            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        var reminderDto = _mapper.Map<ReminderDto>(repositoryResult.Data);

        _logger.LogInformation("Fetched ReminderId: {reminderId}", repositoryResult.Data!.Id);

        return new OkObjectResult(reminderDto);
    }

    #endregion

    #region Post

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ReminderDto reminderDto)
    {
        var reminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        reminderDbo.IntegratorId = HttpContext.GetIntegratorId();

        var repositoryResult = await _remindersRepository.AddAsync(reminderDbo, HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Created ReminderId: {reminderId}", repositoryResult.Data);

        return new ObjectResult(new ReminderIdDto { ReminderId = repositoryResult.Data })
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    #endregion

    #region Patch

    [HttpPatch("setCompleted")]
    public async Task<ActionResult> SetCompleted([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult =
            await _remindersRepository.SetRemindersCompleted(reminderIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful) return new OkResult();

        if (repositoryResult.IsReminderFound is false)
        {
            return new NotFoundResult();
        }

        if (repositoryResult.IsCompletedAlreadySet is true)
        {
            return new ConflictResult();
        }

        throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
    }

    [HttpPatch("decrementRemindCount")]
    public async Task<ActionResult> DecrementRemindCount([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult =
            await _remindersRepository.DecrementRemindCountAsync(reminderIdDto.ReminderId, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful)
            return new OkObjectResult(new RemainRemindCountDto { RemainRemindCount = repositoryResult.Data!.Value });

        if (repositoryResult.IsReminderFound is false)
        {
            return new NotFoundResult();
        }

        throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
    }

    [HttpPatch("reminderDateTime")]
    public async Task<ActionResult> ReminderDateTime([FromBody] SetReminderDateTimeDto setReminderDateTimeDto)
    {
        var repositoryResult =
            await _remindersRepository.SetReminderDateTimeAsync(setReminderDateTimeDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful)
        {
            return new OkResult();
        }

        if (repositoryResult.IsReminderFound is false)
        {
            return new NotFoundResult();
        }

        throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
    }

    #endregion
}