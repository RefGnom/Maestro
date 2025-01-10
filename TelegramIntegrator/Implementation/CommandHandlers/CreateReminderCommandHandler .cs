using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Public.Models.Reminders;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.CommandHandlers
{
    public class CreateReminderCommandHandler(
        ILog<CreateReminderCommandHandler> log,
        IMaestroApiClient maestroApiClient,
        ITelegramBotWrapper telegramBotWrapper,
        IDateTimeProvider dateTimeProvider,
        ITelegramBotClient telegramBotClient
    ) : ICommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILog<CreateReminderCommandHandler> _log = log;
        private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
        private readonly ITelegramBotWrapper _telegramBotWrapper = telegramBotWrapper;
        private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

        public bool CanExecute(ICommand command)
        {
            return command is CreateReminderCommand;
        }

        public async Task ExecuteAsync(long chatId, ICommand command, CancellationToken cancellationToken)
        {
            var reminderCommand = (CreateReminderCommand)command;
            if (reminderCommand.ReminderTime < _dateTimeProvider.GetCurrentDateTime())
            {
                var errorMessage = "Дата напоминания не может быть раньше текущей даты";
                _log.Warn(errorMessage);

                await _telegramBotClient.SendMessage(
                    chatId,
                    errorMessage,
                    cancellationToken: cancellationToken
                );
                return;
            }

            await _maestroApiClient.CreateReminderAsync(
                new ReminderDto
                {
                    UserId = chatId,
                    Description = reminderCommand.Description,
                    RemindDateTime = reminderCommand.ReminderTime,
                    RemindInterval = reminderCommand.RemindInterval,
                    RemindCount = reminderCommand.RemindCount
                }
            );

            _log.Info("Reminder created.");

            await _telegramBotWrapper.SendMainMenu(
                chatId,
                $"Напоминание \"{reminderCommand.Description}\" создано на время {reminderCommand.ReminderTime:dd.MM.yyyy HH:mm}, " +
                $"повторная отправка напоминания (если есть): {reminderCommand.RemindCount} раз(а) через {reminderCommand.RemindInterval.TotalMinutes} мин.",
                cancellationToken
            );
        }
    }
}