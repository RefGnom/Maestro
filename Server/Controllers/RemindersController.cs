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

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var remindersDbo =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _logger.LogInformation("Fetched RemindersCount: {reminderCount}", remindersDbo.Count);

        var remindersDto = _mapper.Map<List<ReminderWithIdDto>>(remindersDbo);

        return new OkObjectResult(remindersDto);
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

    [HttpPost("markAsCompleted")]
    public async Task<ActionResult> MarkAsCompleted([FromBody] RemindersIdDto remindersId)
    {
        await _remindersRepository.MarkAsCompleted(remindersId, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);
        return new OkResult();
    }

    #endregion
}