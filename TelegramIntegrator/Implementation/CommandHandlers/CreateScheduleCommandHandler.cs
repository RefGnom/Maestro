using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.CommandHandlers
{
    public class CreateScheduleCommandHandler(
        ILog<CreateScheduleCommandHandler> log,
        //IMaestroApiClient maestroApiClient,
        IDateTimeProvider dateTimeProvider,
        ITelegramBotClient telegramBotClient
    ) : ICommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILog<CreateScheduleCommandHandler> _log = log;
        //private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
        private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

        public bool CanExecute(ICommand command)
        {
            return command is CreateScheduleCommand;
        }

        public async Task ExecuteAsync(
            long chatId,
            ICommand command,
            CancellationToken cancellationToken
        )
        {
            var scheduleCommand = (CreateScheduleCommand)command;
            if (scheduleCommand.StartDateTime < _dateTimeProvider.GetCurrentDateTime() || scheduleCommand.EndDateTime < _dateTimeProvider.GetCurrentDateTime())
            {
                var errorMessage = "Даты начала и конца расписания не могут быть раньше текущей даты";
                _log.Warn(errorMessage);

                await _telegramBotClient.SendMessage(
                    chatId,
                    errorMessage,
                    cancellationToken: cancellationToken
                );
                return;
            }
            else if (scheduleCommand.StartDateTime > scheduleCommand.EndDateTime)
            {
                var errorMessage = "Дата конца раписание не может быть раньше даты начала расписания.";
                _log.Warn(errorMessage);

                await _telegramBotClient.SendMessage(
                    chatId,
                    errorMessage,
                    cancellationToken: cancellationToken
                );
                return;
            }

            // await _maestroApiClient.CreateScheduleAsync(
            //     new ScheduleDto
            //     {
            //         UserId = chatId,
            //         Description = scheduleCommand.Description,
            //         StartDateTime = scheduleCommand.StartDateTime,
            //         EndDateTime = scheduleCommand.EndDateTime,
            //         CanOverlap = scheduleCommand.CanOverlap
            //     }
            // );

            _log.Info("Schedule created");
            
            await MaestroCommandHandler.SendMainMenu(_telegramBotClient, chatId,
                $"Расписание \"{scheduleCommand.Description}\" создано на время с {scheduleCommand.StartDateTime:dd.MM.yyyy HH:mm} " +
                $"по {scheduleCommand.EndDateTime:dd.MM.yyyy HH:mm}.",
                cancellationToken);
        }
    }
}