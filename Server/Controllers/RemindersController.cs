using System.Net;
using AutoMapper;
using Maestro.Core.Logging;
using Maestro.Data.Models;
using Maestro.Server.Core.Models;
using Maestro.Server.Extensions;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[ApiController]
[Route("api/v1/reminders")]
public class RemindersController(IRemindersRepository remindersRepository, IMapper mapper, ILogFactory logFactory) : ControllerBase
{
    private readonly ILog<RemindersController> _log = logFactory.CreateLog<RemindersController>();
    private readonly IMapper _mapper = mapper;
    private readonly IRemindersRepository _remindersRepository = remindersRepository;

    #region Post

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ReminderDto reminderDto)
    {
        var reminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        reminderDbo.IntegratorId = HttpContext.GetIntegratorId();

        var createdReminderId = await _remindersRepository.AddAsync(reminderDbo, HttpContext.RequestAborted);

        _log.Info($"Created reminder Id: {createdReminderId}");

        return new ObjectResult(new ReminderIdDto { Id = createdReminderId })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    #endregion

    #region Get

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var remindersDbo =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _log.Info($"Fetched reminders Count: {remindersDbo.Count}");

        var remindersDto = _mapper.Map<List<ReminderDtoWithId>>(remindersDbo);

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

        _log.Info($"Fetched reminder Id: {reminderDbo.Id}");

        return new ObjectResult(reminderDto);
    }

    #endregion
}