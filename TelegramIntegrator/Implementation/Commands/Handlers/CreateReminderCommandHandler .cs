using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Reminders;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public class CreateReminderCommandHandler(
    ILog<CreateReminderCommandHandler> log,
    IMaestroApiClient maestroApiClient,
    IDateTimeProvider dateTimeProvider,
    ITelegramBotClient telegramBotClient
) : CommandHandlerBase<CreateReminderCommandModel>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILog<CreateReminderCommandHandler> _log = log;
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override string CommandName => TelegramCommandNames.CreateReminder;

    protected async override Task ExecuteAsync(
        ChatContext context,
        CreateReminderCommandModel reminderCommandModel
    )
    {
        if (reminderCommandModel.ReminderTime < _dateTimeProvider.GetCurrentDateTime())
        {
            var errorMessage = "Дата напоминания не может быть раньше текущей даты";
            _log.Warn(errorMessage);

            await _telegramBotClient.SendMessage(
                context.ChatId,
                errorMessage
            );
            return;
        }

        await _maestroApiClient.CreateReminderAsync(
            new ReminderDto
            {
                UserId = context.UserId,
                Description = reminderCommandModel.ReminderDescription,
                RemindDateTime = reminderCommandModel.ReminderTime,
                RemindInterval = reminderCommandModel.RemindInterval,
                RemindCount = reminderCommandModel.RemindCount
            }
        );

        _log.Info("Reminder created.");

        await _telegramBotClient.SendMessage(
            context.ChatId,
            $"Напоминание \"{reminderCommandModel.HelpDescription}\" создано на время {reminderCommandModel.ReminderTime:dd.MM.yyyy HH:mm}, " +
            $"повторная отправка напоминания (если есть): {reminderCommandModel.RemindCount} раз(а) через {reminderCommandModel.RemindInterval.TotalMinutes} мин."
        );
    }
}