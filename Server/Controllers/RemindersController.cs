using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Authentication;
using Maestro.Server.Core.Models;
using Maestro.Server.Extensions;
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
        var remindersDbos =
            await _remindersRepository.GetAllRemindersAsync(allRemindersDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        var remindersWithIdsDtos = _mapper.Map<List<ReminderWithIdDto>>(remindersDbos);

        return new OkObjectResult(remindersWithIdsDtos);
    }

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var remindersDbo =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _logger.LogInformation("Fetched RemindersCount: {reminderCount}", remindersDbo.Count);

        var remindersWithIdsDtos = _mapper.Map<List<ReminderWithIdDto>>(remindersDbo);

        return new OkObjectResult(remindersWithIdsDtos);
    }

    [HttpGet("byId")]
    public async Task<ActionResult> ById([FromBody] ReminderIdDto reminderIdDto)
    {
        var reminderDbo = await _remindersRepository.GetByIdAsync(reminderIdDto.Id, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (reminderDbo is null)
        {
            return new NotFoundResult();
        }

        var reminderDto = _mapper.Map<ReminderDto>(reminderDbo);

        _logger.LogInformation("Fetched ReminderId: {reminderId}", reminderDbo.Id);

        return new OkObjectResult(reminderDto);
    }

    #endregion

    #region Post

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ReminderDto reminderDto)
    {
        var reminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        reminderDbo.IntegratorId = HttpContext.GetIntegratorId();

        var createdReminderId = await _remindersRepository.AddAsync(reminderDbo, HttpContext.RequestAborted);

        _logger.LogInformation("Created ReminderId: {reminderId}", createdReminderId);

        return new ObjectResult(new ReminderIdDto { Id = createdReminderId })
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpPatch("markAsCompleted")]
    public async Task<ActionResult> MarkAsCompleted([FromBody] RemindersIdsDto remindersIds)
    {
        var notFoundRemindersIds =
            await _remindersRepository.MarkAsCompleted(remindersIds, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (notFoundRemindersIds is not null)
        {
            return new NotFoundObjectResult(new RemindersIdsDto { RemindersIds = notFoundRemindersIds });
        }

        return new OkResult();
    }

    [HttpPatch("decrementRemindCount")]
    public async Task<ActionResult> DecrementRemindCount([FromBody] ReminderIdDto reminderIdDto)
    {
        var reminderCount =
            await _remindersRepository.DecrementRemindCountAsync(reminderIdDto.Id, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        if (reminderCount is null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(new ReminderCountDto { ReminderCount = reminderCount.Value });
    }

    #endregion
}