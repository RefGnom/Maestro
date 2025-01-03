using System.Net;
using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Authorization;
using Maestro.Server.Core.Models;
using Maestro.Server.Extensions;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationSchemes = Maestro.Server.Authentication.AuthenticationSchemes;

namespace Maestro.Server.Controllers;

[Authorize(Policy = AuthorizationPolicies.Integrator, AuthenticationSchemes = AuthenticationSchemes.ApiKey)]
[ApiController]
[Route("api/v1/reminders")]
public class RemindersController(IRemindersRepository remindersRepository, IMapper mapper, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(nameof(RemindersController));
    private readonly IMapper _mapper = mapper;
    private readonly IRemindersRepository _remindersRepository = remindersRepository;

    #region Post

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ReminderDto reminderDto)
    {
        var reminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        reminderDbo.IntegratorId = HttpContext.GetIntegratorId();

        var createdReminderId = await _remindersRepository.AddAsync(reminderDbo, HttpContext.RequestAborted);

        _logger.LogInformation("Created reminder Id: {reminderId}", createdReminderId);

        return new ObjectResult(new ReminderIdDto { Id = createdReminderId })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    [HttpPost("markAsCompleted")]
    public async Task<ActionResult> MarkAsCompleted([FromBody] long[] remindersId)
    {
        await _remindersRepository.MarkAsCompleted(remindersId, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _log.Info($"Marked reminders as completed. ReminderIds: {string.Join(", ", remindersId)}");

        return NoContent();
    }

    #endregion

    #region Get

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var remindersDbo =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _logger.LogInformation("Fetched reminders Count: {reminderCount}", remindersDbo.Count);

        var remindersDto = _mapper.Map<List<ReminderWithIdDto>>(remindersDbo);

        return new ObjectResult(remindersDto);
    }

    [HttpGet("byId")]
    public async Task<ActionResult> ById([FromBody] ReminderIdDto reminderIdDto)
    {
        var reminderDbo = await _remindersRepository.GetByIdAsync(reminderIdDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (reminderDbo is null)
        {
            return new NotFoundResult();
        }

        var reminderDto = _mapper.Map<ReminderDto>(reminderDbo);

        _logger.LogInformation("Fetched reminder Id: {reminderId}", reminderDbo.Id);

        return new ObjectResult(reminderDto);
    }

    [HttpGet("forUserInTimeRange")]
    public async Task<ActionResult> ForUserInTimeRange([FromBody] RemindersForUserDtoWithTimeRange remindersForUserDto)
    {
        var remindersDbo =
            await _remindersRepository.GetForUserInTimeRangeAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _log.Info($"Fetched reminders Count: {remindersDbo.Count}");

        var remindersDto = _mapper.Map<List<ReminderDtoWithId>>(remindersDbo);

        return new ObjectResult(remindersDto);
    }

    #endregion
}