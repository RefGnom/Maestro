using System.Net;
using AutoMapper;
using Maestro.Core.Logging;
using Maestro.Data.Models;
using Maestro.Server.Core.ApiModels;
using Maestro.Server.Extensions;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[ApiController]
[Route("api/v1/reminders")]
public class RemindersController(IRemindersRepository remindersRepository, IMapper mapper, ILogFactory logFactory) : ControllerBase
{
    private readonly IRemindersRepository _remindersRepository = remindersRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILog<RemindersController> _log = logFactory.CreateLog<RemindersController>();

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser([FromBody] RemindersForUserDto remindersForUserDto)
    {
        var remindersDboList =
            await _remindersRepository.GetForUserAsync(remindersForUserDto, HttpContext.GetIntegratorId(), HttpContext.RequestAborted);

        _log.Info($"Fetched reminders Count: {remindersDboList.Count}");

        var remindersDtoList = _mapper.Map<List<ReminderDtoWithId>>(remindersDboList);

        return new ObjectResult(remindersDtoList);
    }

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
}