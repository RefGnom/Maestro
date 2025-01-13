using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Schedules;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public class CreateScheduleCommandHandler(
    ILog<CreateScheduleCommandHandler> log,
    IMaestroApiClient maestroApiClient,
    IDateTimeProvider dateTimeProvider,
    ITelegramBotClient telegramBotClient
) : CommandHandlerBase<CreateScheduleCommandModel>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILog<CreateScheduleCommandHandler> _log = log;
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override string CommandName => TelegramCommandNames.CreateSchedule;

    protected async override Task ExecuteAsync(
        ChatContext context,
        CreateScheduleCommandModel scheduleCommandModel
    )
    {
        if (scheduleCommandModel.StartDateTime < _dateTimeProvider.GetCurrentDateTime())
        {
            var errorMessage = "Даты начала расписания не может быть раньше текущей даты";
            _log.Warn(errorMessage);

            await _telegramBotClient.SendMessage(
                context.ChatId,
                errorMessage
            );
            return;
        }

        var scheduleId = await _maestroApiClient.CreateScheduleAsync(
            new ScheduleDto
            {
                UserId = context.UserId,
                Description = scheduleCommandModel.ScheduleDescription,
                StartDateTime = scheduleCommandModel.StartDateTime,
                Duration = scheduleCommandModel.Duration,
                CanOverlap = scheduleCommandModel.CanOverlap,
                IsCompleted = false
            }
        );
        if (scheduleId is null)
        {
            await _telegramBotClient.SendMessage(
                context.ChatId,
                $"Невозможно создать расписание \"{scheduleCommandModel.ScheduleDescription}\", потому что оно имеет пересечения"
            );
            return;
        }

        await _telegramBotClient.SendMessage(
            context.ChatId,
            $"Расписание \"{scheduleCommandModel.ScheduleDescription}\" создано на {scheduleCommandModel.StartDateTime:dd.MM.yyyy HH:mm} " +
            $"с продолжительностью {scheduleCommandModel.Duration}."
        );
    }
}