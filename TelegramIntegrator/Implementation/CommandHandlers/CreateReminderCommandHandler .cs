using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.CommandHandlers
{
    public class CreateReminderCommandHandler(
        ILog<CreateReminderCommandHandler> log,
        //IMaestroApiClient maestroApiClient,
        IDateTimeProvider dateTimeProvider,
        ITelegramBotClient telegramBotClient
    ) : ICommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILog<CreateReminderCommandHandler> _log = log;
        //private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
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
                var errorMessage = "Reminder time is less than current time";
                _log.Warn(errorMessage);

                await _telegramBotClient.SendMessage(
                    chatId,
                    errorMessage,
                    cancellationToken: cancellationToken
                );
                return;
            }

            //await _maestroApiClient.CreateReminderAsync(
            //    new ReminderDto
            //    {
            //        UserId = chatId,
            //        Description = reminderCommand.Description,
            //        ReminderTime = reminderCommand.ReminderTime,
            //        RemindInterval = TimeSpan.Zero,
            //        RemindCount = 1
            //    }
            //);

            _log.Info("Reminder created.");
            await _telegramBotClient.SendMessage(
                chatId,
                $"Напоминание \"{reminderCommand.Description}\" создано на время {reminderCommand.ReminderTime:yyyy-MM-dd HH:mm}",
                cancellationToken: cancellationToken
            );
        }
    }
}