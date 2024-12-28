using AutoMapper;
using Maestro.Core.Models;
using Maestro.Data.Models;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[ApiController]
[Route("api/v1/reminders")]
public class RemindersController(IRemindersRepository remindersRepository, IMapper mapper) : ControllerBase
{
    private readonly IRemindersRepository _remindersRepository = remindersRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("forUser")]
    public async Task<ActionResult> ForUser(long userId)
    {
        var reminders = await _remindersRepository.GetForUser(userId, HttpContext.RequestAborted);
        return new ObjectResult(reminders);
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] ReminderDto reminderDto)
    {
        var reminderDbo = _mapper.Map<ReminderDbo>(reminderDto);
        await _remindersRepository.AddAsync(reminderDbo, HttpContext.RequestAborted);
        return Created();
    }
}