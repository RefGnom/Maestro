using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.CommandHandlers;
using Maestro.TelegramIntegrator.Parsers.CommandParsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    IEnumerable<ICommandParser> commandParsers,
    IEnumerable<ICommandHandler> commandHandlers
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly ICommandParser[] _commandParsers = commandParsers.ToArray();
    private readonly ICommandHandler[] _commandHandlers = commandHandlers.ToArray();

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message is not null)
        {
            foreach (var parser in _commandParsers)
            {
                if (parser.CanParse(update.Message.Text!))
                {
                    var commandParseResult = parser.ParseCommand(update.Message.Text!);

                    if (!commandParseResult.IsSuccessful)
                    {
                        _log.Warn("Failed to parse message");
                        await bot.SendMessage(update.Message.Chat.Id, commandParseResult.ParseFailureMessage, cancellationToken: cancellationToken);
                        return;
                    }

                    var command = commandParseResult.Value;

                    foreach (var handler in _commandHandlers)
                    {
                        if (handler.CanExecute(command))
                        {
                            await handler.ExecuteAsync(update.Message.Chat.Id, command, cancellationToken);
                            return;
                        }
                    }

                    throw new Exception($"Не нашли подходящего CommandHandler для команды {command}");
                }
            }

            await SendMainMenu(bot, update.Message.Chat.Id, "Неизвестная команда.", cancellationToken); ;
        }

        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
        {
            var callbackQuery = update.CallbackQuery;

            switch (callbackQuery.Data)
            {
                case "create_reminder":
                    await bot.SendMessage(callbackQuery.Message!.Chat.Id,
                        "Введите детали для создания напоминания через запятую:\n" +
                        "- команда /reminder,\n- дата и время через пробел в формате \"день.месяц.год часы:минуты\",\n- текст напоминания,\n" +
                        "- количество отправки напоминания - если хотите получить напоминание несколько раз, чтобы точно не забыть",
                        cancellationToken: cancellationToken);

                    break;

                case "create_schedule":
                    await bot.SendMessage(callbackQuery.Message!.Chat.Id,
                        "Введите детали для создания расписания через запятую:\n" +
                        "- команда /schedule,\nдата и время начала расписания через пробел в формате \"день.месяц.год часы:минуты\",\n" +
                        "- дата и время конца расписания через пробел в формате \"день.месяц.год часы:минуты\",\n- текст расписания,\n" +
                        "- параметр \"overlap\" - если это расписание может пересекаться с другими", cancellationToken: cancellationToken);
                    break;

                //case "view_reminders":
                //    var reminders = await _maestroApiClient.GetRemindersForUserAsync(callbackQuery.Message.Chat.Id);
                //    await bot.SendMessage(callbackQuery.Message.Chat.Id, $"Ваши напоминания:\n{string.Join("\n", reminders)}", cancellationToken: cancellationToken);
                //    break;

                //case "view_schedules":
                //    var schedules = await _maestroApiClient.GetSchedulesForUserAsync(callbackQuery.Message.Chat.Id);
                //    await bot.SendMessage(callbackQuery.Message.Chat.Id, $"Ваши расписания:\n{string.Join("\n", schedules)}", cancellationToken: cancellationToken);
                //    break;
            }
        }
    }

    public static async Task SendMainMenu(ITelegramBotClient bot, long chatId, string message, CancellationToken cancellationToken)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithCallbackData("Создать напоминание", "create_reminder")
            ],
            [
                InlineKeyboardButton.WithCallbackData("Создать расписание", "create_schedule")
            ],
            //[
            //    InlineKeyboardButton.WithCallbackData("Посмотреть мои напоминания", "get_reminders")
            //],
            //[
                
            //    InlineKeyboardButton.WithCallbackData("Посмотреть мои расписания", "get_schedules")
            //]
        ]);

        await bot.SendMessage(chatId, message + " Выберите действие:", replyMarkup: inlineKeyboardMarkup, cancellationToken: cancellationToken);
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}