using Maestro.Client;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation;

public class CallbackQueryHandler(ITelegramBotClient telegramBotClient)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public readonly Dictionary<string, Func<long, ITelegramBotClient, CancellationToken, Task>> CallbackDataHandlers = new()
    {
        {
            "create_reminder", (chatId, bot, cancellationToken) =>
                bot.SendMessage(
                    chatId,
                    "Введите детали для создания напоминания через запятую:\nОбязательные детали:\n" +
                    "- команда /reminder,\n- дата и время через пробел в формате \"день.месяц.год часы:минуты\",\n- текст напоминания,\n" +
                    "Необязательные параметры:\n" +
                    "- количество повторной отправки напоминания - если хотите получить напоминание несколько раз, чтобы точно не забыть,\n" +
                    "- интервал между повторной отправкой в формате \"часы:минуты:секунды\" (по умолчанию 5 минут) " +
                    "- если хотите поменять значение интервала по умолчанию",
                    cancellationToken: cancellationToken
                )
        },
        {
            "create_schedule", (chatId, bot, cancellationToken) =>
                bot.SendMessage(
                    chatId,
                    "Введите детали для создания расписания через запятую:\n" +
                    "- команда /schedule,\n- дата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                    "- дата и время конца расписания через пробел в формате \"день.месяц.год часы:минуты\",\n- текст расписания,\n" +
                    "- параметр \"overlap\" - если это расписание может пересекаться с другими",
                    cancellationToken: cancellationToken
                )
        }

        //{ "view_reminders", async (chatId, bot, cancellationToken) =>
        //    {
        //        var reminders = _maestroApiClient.GetRemindersForUserAsync(chatId, null);
        //        await bot.SendMessage(chatId,
        //                $"Ваши напоминания:\n{string.Join("\n", reminders)}",
        //                cancellationToken: cancellationToken);
        //    }},

        //{ "view_schedules", async (chatId, bot, cancellationToken) =>
        //    {
        //        var schedules = _maestroApiClient.GetSchedulesForUserAsync(chatId);
        //        await bot.SendMessage(chatId,
        //                "У вас нет активных расписаний.",
        //                cancellationToken: cancellationToken);
        //    }}
    };
        
    public async Task HandleCallbackData(string callbackData, long chatId, CancellationToken cancellationToken)
    {
        if (CallbackDataHandlers.TryGetValue(callbackData, out var handler))
        {
            await handler(chatId, _telegramBotClient, cancellationToken);
        }
        else
        {
            throw new Exception($"Не нашли подходящего хэндлера для команды {callbackData}");
        }
    }
}