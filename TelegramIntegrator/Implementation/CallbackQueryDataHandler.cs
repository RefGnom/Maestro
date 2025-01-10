using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation
{
    public class CallbackQueryHandler
    {
        //private readonly IMaestroApiClient _maestroApiClient;
        private readonly ITelegramBotClient _telegramBotClient;

        public Dictionary<string, Func<long, CancellationToken, Task>> CallbackDataHandlers { get; }

        public CallbackQueryHandler(
            //IMaestroApiClient maestroApiClient,
            ITelegramBotClient telegramBotClient)
        {
            //_maestroApiClient = maestroApiClient;
            _telegramBotClient = telegramBotClient;

            CallbackDataHandlers = new()
            {
                { "create_reminder", (chatId, cancellationToken) =>
                    _telegramBotClient.SendMessage(chatId,
                            "Введите детали для создания напоминания через запятую:\nОбязательные детали:\n" +
                            "- команда /reminder,\n- дата и время через пробел в формате \"день.месяц.год часы:минуты\",\n- текст напоминания,\n" +
                            "Необязательные параметры:\n" +
                            "- количество повторной отправки напоминания - если хотите получить напоминание несколько раз, чтобы точно не забыть,\n" +
                            "- интервал между повторной отправкой в формате \"часы:минуты:секунды\" (по умолчанию 5 минут) " +
                            "- если хотите поменять значение интервала по умолчанию",
                            cancellationToken: cancellationToken)},

                { "create_schedule", (chatId, cancellationToken) =>
                    _telegramBotClient.SendMessage(chatId,
                            "Введите детали для создания расписания через запятую:\n" +
                            "- команда /schedule,\n- дата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                            "- дата и время конца расписания через пробел в формате \"день.месяц.год часы:минуты\",\n- текст расписания,\n" +
                            "- параметр \"overlap\" - если это расписание может пересекаться с другими",
                            cancellationToken: cancellationToken)},

                //{ "view_reminders", async (chatId, cancellationToken) =>
                //    {
                //        var reminders = _maestroApiClient.GetRemindersForUserAsync(chatId, null);
                //        await _telegramBotClient.SendMessage(chatId,
                //                $"Ваши напоминания:\n{string.Join("\n", reminders)}",
                //                cancellationToken: cancellationToken);
                //    }},

                //{ "view_schedules", async (chatId, cancellationToken) =>
                //    {
                //        var schedules = _maestroApiClient.GetSchedulesForUserAsync(chatId);
                //        await _telegramBotClient.SendMessage(chatId,
                //                "У вас нет активных расписаний.",
                //                cancellationToken: cancellationToken);
                //    }}
            };
        }

        public async Task HandleCallbackData(string callbackData, long chatId, CancellationToken cancellationToken)
        {
            if (CallbackDataHandlers.TryGetValue(callbackData, out var handler))
            {
                await handler(chatId, cancellationToken);
            }
            else
            {
                throw new Exception($"Не нашли подходящего хэндлера для команды {callbackData}");
            }

            ;
        }
    }
}
