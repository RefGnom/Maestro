using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers
{
    public class ViewRemindersCommandHandler(
    ILog<CreateReminderCommandHandler> log,
    IMaestroApiClient maestroApiClient,
    ITelegramBotClient telegramBotClient
) : CommandHandlerBase<ViewRemindersCommandModel>
    {
        private readonly ILog<CreateReminderCommandHandler> _log = log;
        private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
        private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

        public override string CommandName => TelegramCommandNames.ViewReminders;

        protected async override Task ExecuteAsync(
            ChatContext context,
            ViewRemindersCommandModel viewRemindersCommandModel
        )
        {
            var reminders = _maestroApiClient.GetRemindersForUserAsync(context.UserId, null);

            var remindersList = new List<string>();

            await foreach (var reminder in reminders)
            {
                remindersList.Add($"{reminder.Description},  {reminder.RemindDateTime:dd.mm.yyyy} в {reminder.RemindDateTime.TimeOfDay}");
            }

            if (remindersList.Count != 0)
            {
                await _telegramBotClient.SendMessage(context.UserId,
                $"Ваши напоминания:\n\n{string.Join("\n", remindersList)}"
                );

                _log.Info($"Sent reminders to the user");
                return;
            }

            await _telegramBotClient.SendMessage(context.UserId,
                "Напоминаний не найдено"
            );
        }
    }
}
