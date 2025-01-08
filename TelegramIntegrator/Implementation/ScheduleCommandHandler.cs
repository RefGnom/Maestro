using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation
{
    public class ScheduleCommandHandler(
    ILog<ScheduleCommandHandler> log,
    //IMaestroApiClient maestroApiClient,
    IDateTimeProvider dateTimeProvider
    ) : ICommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILog<ScheduleCommandHandler> _log = log;
        //private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;

        public bool CanExecute(ICommand command)
        {
            return command is ScheduleCommand;
        }

        public async Task ExecuteAsync(ITelegramBotClient bot, long chatId, ICommand command, CancellationToken cancellationToken)
        {
            var scheduleCommand = (ScheduleCommand)command;
            if (scheduleCommand.StartDateTime < _dateTimeProvider.GetCurrentDateTime() || scheduleCommand.StartDateTime < _dateTimeProvider.GetCurrentDateTime())
            {
                var errorMessage = "Start or end time of the schedule is less than the current time";
                _log.Warn(errorMessage);

                await bot.SendMessage(
                chatId,
                errorMessage,
                    cancellationToken: cancellationToken
                );
            }

            //await _maestroApiClient.CreateScheduleAsync(new ScheduleDto
            //{
            //    UserId = chatId,
            //    Description = scheduleCommand.Description,
            //    StartDateTime = scheduleCommand.StartDateTime,
            //    EndDateTime = scheduleCommand.EndDateTime,
            //    CanOverlap = scheduleCommand.CanOverlap
            //});

            _log.Info("Schedule created");
            await bot.SendMessage(chatId,
                $"Расписание \"{scheduleCommand.Description}\" создано на время с {scheduleCommand.StartDateTime:yyyy-MM-dd HH:mm} по {scheduleCommand.EndDateTime:yyyy-MM-dd HH:mm}",
                cancellationToken: cancellationToken);
        }
    }
}