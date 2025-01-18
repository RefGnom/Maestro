using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers
{
    public class ViewSchedulesCommandHandler(
    ILog<ViewSchedulesCommandHandler> log,
    IMaestroApiClient maestroApiClient,
    ITelegramBotClient telegramBotClient
) : CommandHandlerBase<ViewSchedulesCommandModel>
    {
        private readonly ILog<ViewSchedulesCommandHandler> _log = log;
        private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
        private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

        public override string CommandName => TelegramCommandNames.ViewSchedules;

        protected async override Task ExecuteAsync(
            ChatContext context,
            ViewSchedulesCommandModel viewSchedulesCommandModel
        )
        {
            var schedules = _maestroApiClient.GetSchedulesForUserAsync(context.UserId, null);

            var schedulesList = new List<string>();

            await foreach (var schedule in schedules)
            {
                schedulesList.Add($"{schedule.Description}, начало: {schedule.StartDateTime:dd.mm.yyyy}, " +
                    $"продолжительность: {schedule.Duration.Days} д {schedule.Duration.Hours} ч {schedule.Duration.Minutes} м.");
            }

            if (schedulesList.Any())
            {
                await _telegramBotClient.SendMessage(context.UserId,
                $"Ваши расписания:\n\n{string.Join("\n", schedulesList)}"
                );

                _log.Info($"Sent schedules to the user");
            }
            else
            {
                await _telegramBotClient.SendMessage(context.UserId,
                "Расписаний не найдено"
                );
            }
        }
    }
}
