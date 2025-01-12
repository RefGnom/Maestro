using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Reminders;
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
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();
        if (scheduleCommandModel.StartDateTime < currentDateTime || scheduleCommandModel.EndDateTime < currentDateTime)
        {
            var errorMessage = "Даты начала и конца расписания не могут быть раньше текущей даты";
            _log.Warn(errorMessage);

            await _telegramBotClient.SendMessage(
                context.ChatId,
                errorMessage
            );
            return;
        }

        if (scheduleCommandModel.StartDateTime > scheduleCommandModel.EndDateTime)
        {
            var errorMessage = "Дата конца раписание не может быть раньше даты начала расписания.";
            _log.Warn(errorMessage);

            await _telegramBotClient.SendMessage(
                context.ChatId,
                errorMessage
            );
            return;
        }

        await _maestroApiClient.CreateScheduleAsync(
            new ScheduleDto
            {
                UserId = context.UserId,
                Description = scheduleCommandModel.ScheduleDescription,
                StartDateTime = scheduleCommandModel.StartDateTime,
                EndDateTime = scheduleCommandModel.EndDateTime,
                CanOverlap = scheduleCommandModel.CanOverlap
            }
        );

        _log.Info("Schedule created");

        await _telegramBotClient.SendMessage(
            context.ChatId,
            $"Расписание \"{scheduleCommandModel.HelpDescription}\" создано на время с {scheduleCommandModel.StartDateTime:dd.MM.yyyy HH:mm} " +
            $"по {scheduleCommandModel.EndDateTime:dd.MM.yyyy HH:mm}."
        );
    }
}