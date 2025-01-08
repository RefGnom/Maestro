using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation
{
    public class ReminderCommandHandler(
    ILog<ReminderCommandHandler> log,
    //IMaestroApiClient maestroApiClient,
    IDateTimeProvider dateTimeProvider
    ) : ICommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILog<ReminderCommandHandler> _log = log;
        //private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;

        public bool CanExecute(ICommand command)
        {
            return command is ReminderCommand;
        }

        public async Task ExecuteAsync(ITelegramBotClient bot, long chatId, ICommand command, CancellationToken cancellationToken)
        {
            var reminderCommand = (ReminderCommand)command;
            if (reminderCommand.ReminderTime < _dateTimeProvider.GetCurrentDateTime())
            {
                var errorMessage = "Reminder time is less than current time";
                _log.Warn(errorMessage);

                await bot.SendMessage(
                chatId,
                errorMessage,
                    cancellationToken: cancellationToken
                );
            }

            //await _maestroApiClient.CreateReminderAsync(
            //new ReminderDto
            //{
            //    UserId = chatId,
            //    Description = reminderCommand.Description,
            //    ReminderTime = reminderCommand.ReminderTime,
            //    RemindInterval = TimeSpan.Zero,
            //    RemindCount = 1
            //});

            _log.Info("Reminder created.");
            await bot.SendMessage(chatId,
                $"Напоминание \"{reminderCommand.Description}\" создано на время {reminderCommand.ReminderTime:yyyy-MM-dd HH:mm}",
                cancellationToken: cancellationToken);
        }
    }
}
