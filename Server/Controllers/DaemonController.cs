using AutoMapper;
using Maestro.Server.Authentication;
using Maestro.Server.Extensions;
using Maestro.Server.Helpers;
using Maestro.Server.Private.Authentication;
using Maestro.Server.Private.Models;
using Maestro.Server.Public.Models.Reminders;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[Authorize(Roles = ServiceRoles.Daemon, AuthenticationSchemes = AuthenticationSchemes.DaemonApiKey)]
[ApiController]
[Route("daemon")]
public class DaemonController(IRemindersRepository remindersRepository, IMapper mapper, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DaemonController>();
    private readonly IMapper _mapper = mapper;
    private readonly IRemindersRepository _remindersRepository = remindersRepository;

    #region Get

    [HttpGet("completedReminders")]
    public async Task<ActionResult> CompletedReminders([FromQuery] CompletedRemindersDto completedRemindersDto)
    {
        var repositoryResult =
            await _remindersRepository.GetCompletedRemindersAsync(completedRemindersDto, HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        _logger.LogInformation("Fetched RemindersCount: {remindersCount}", repositoryResult.Data!.Count);

        var reminderWithIdDtos = _mapper.Map<List<ReminderWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(reminderWithIdDtos);
    }

    [HttpGet("oldReminders")]
    public async Task<ActionResult> OldReminders([FromQuery] OldRemindersDto oldRemindersDto)
    {
        var repositoryResult =
            await _remindersRepository.GetOldRemindersAsync(oldRemindersDto, HttpContext.RequestAborted);

        if (!repositoryResult.IsSuccessful)
        {
            RepositoryThrowHelper.ThrowUnexpectedRepositoryResult();
        }

        var reminderWithIdDtos = _mapper.Map<List<ReminderWithIdDto>>(repositoryResult.Data);

        return new OkObjectResult(reminderWithIdDtos);
    }

    #endregion

    #region Delete

    [HttpDelete("reminder")]
    public async Task<ActionResult> Reminder([FromBody] ReminderIdDto reminderIdDto)
    {
        var repositoryResult = await _remindersRepository.DeleteReminderByIdAsync(reminderIdDto, HttpContext.RequestAborted);

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