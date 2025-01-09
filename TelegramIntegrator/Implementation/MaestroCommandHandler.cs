using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.CommandHandlers;
using Maestro.TelegramIntegrator.Parsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    ICommandParser commandParser,
    IEnumerable<ICommandHandler> commandHandlers
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly ICommandParser _commandParser = commandParser;
    private readonly ICommandHandler[] _commandHandlers = commandHandlers.ToArray();

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message is not null)
        {
            var commandParseResult = _commandParser.ParseCommand(update.Message.Text!);
            if (!commandParseResult.IsSuccessful)
            {
                _log.Warn("Failed to parse message");
                await SendMainMenu(bot, update.Message.Chat.Id, commandParseResult.ParseFailureMessage, cancellationToken);
                
                return;
            }

            var command = commandParseResult.Command;

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

        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
        {
            var callbackQuery = update.CallbackQuery;

            switch (callbackQuery.Data)
            {
                case "create_reminder":
                    await bot.SendMessage(callbackQuery.Message.Chat.Id,
                        "Введите детали для создания напоминания через запятую без пробелов в формате: " +
                        "команда /reminder,дата в формате день.месяц.год,время в формате часы:минуты,текст напоминания", cancellationToken: cancellationToken);

                    break;

                case "create_schedule":
                    await bot.SendMessage(callbackQuery.Message.Chat.Id,
                        "Введите детали для создания расписания через запятую без пробелов в формате: " +
                        "команда /schedule, дата события в формате день.месяц.год, время начала события в формате часы:минуты, время конца события в формате часы:минуты," +
                        "текст события,параметр \"overlap\" - если это расписание может пересекаться с другими", cancellationToken: cancellationToken);
                    break;

                //case "view_reminders":
                //    var reminders = await _maestroApiClient.GetRemindersForUserAsync(callbackQuery.Message.Chat.Id);
                //    await bot.SendMessage(callbackQuery.Message.Chat.Id, $"Ваши напоминания:\n{string.Join("\n", reminders)}", cancellationToken: cancellationToken);
                //    break;

                //case "view_schedules":
                //    var schedules = await _maestroApiClient.GetSchedulesForUserAsync(callbackQuery.Message.Chat.Id);
                //    await bot.SendMessage(callbackQuery.Message.Chat.Id, $"Ваши расписания:\n{string.Join("\n", schedules)}", cancellationToken: cancellationToken);
                //    break;

                default:
                    await SendMainMenu(bot, update.Message.Chat.Id, "Неизвестная команда.", cancellationToken);
                    break;
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
            [
                InlineKeyboardButton.WithCallbackData("Посмотреть мои напоминания", "get_reminders")
            ],
            [
                
                InlineKeyboardButton.WithCallbackData("Посмотреть мои расписания", "get_schedules")
            ]
        ]);

        await bot.SendMessage(chatId, message + " Выберите действие:", replyMarkup: inlineKeyboardMarkup, cancellationToken: cancellationToken);
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}