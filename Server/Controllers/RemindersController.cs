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

[Authorize(Roles = IntegratorsRoles.Base, AuthenticationSchemes = AuthenticationSchemes.IntegratorApiKey)]
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

        _logger.LogInformation("Fetched RemindersCount: {remindersCount}", repositoryResult.Data!.Count);

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

        var reminderWithIdDtos = _mapper.Map<List<ReminderWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(reminderWithIdDtos);
    }

    [HttpGet("byId")]
    public async Task<ActionResult> ById([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult =
            await _remindersRepository.GetByIdAsync(reminderIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            if (repositoryResult.IsReminderFound is false)
            {
                _logger.LogWarning("Operation failed. Reminder was not found. ReminderId: {reminderId}", reminderIdDto.ReminderId);
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
        var newReminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        newReminderDbo.IntegratorId = HttpContext.GetIntegratorId();

        var repositoryResult = await _remindersRepository.AddAsync(newReminderDbo, HttpContext.RequestAborted);

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
            await _remindersRepository.SetReminderCompleted(reminderIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful) return new OkResult();

        if (repositoryResult.IsReminderFound is false)
        {
            _logger.LogWarning("Operation failed. Reminder was not found. ReminderId: {reminderId}", reminderIdDto.ReminderId);
            return new NotFoundResult();
        }

        if (repositoryResult.IsCompletedAlreadySet is not true)
        {
            throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
        }

        _logger.LogWarning("Operation failed. Completed already set");
        return new ConflictResult();
    }

    [HttpPatch("decrementRemindCount")]
    public async Task<ActionResult> DecrementRemindCount([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult =
            await _remindersRepository.DecrementRemindCountAsync(reminderIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful)
        {
            _logger.LogInformation("Reminder RemindCount decremented");
            return new OkObjectResult(new RemainRemindCountDto { RemainRemindCount = repositoryResult.Data!.Value });
        }

        if (repositoryResult.IsReminderFound is false)
        {
            _logger.LogWarning("Operation failed. Reminder was not found. ReminderId: {reminderId}", reminderIdDto.ReminderId);
            return new NotFoundResult();
        }

        if (repositoryResult.IsRemindCountEqualZero is not true)
        {
            throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
        }

        _logger.LogWarning("Operation failed. RemindCount is zero");
        return new ConflictResult();
    }

    [HttpPatch("reminderDateTime")]
    public async Task<ActionResult> ReminderDateTime([FromBody] ReminderDateTimeDto reminderDateTimeDto)
    {
        var repositoryResult =
            await _remindersRepository.SetReminderDateTimeAsync(reminderDateTimeDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (repositoryResult.IsSuccessful)
        {
            _logger.LogInformation("RemindDateTime set");
            return new OkResult();
        }

        if (repositoryResult.IsReminderFound is not false)
        {
            throw RepositoryThrowHelper.GetUnexpectedRepositoryResultException();
        }

        _logger.LogWarning("Operation failed. Reminder was not found. ReminderId: {reminderId}", reminderDateTimeDto.ReminderId);

        return new NotFoundResult();
    }

    #endregion
}