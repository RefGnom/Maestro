using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.CommandHandlers;
using Maestro.TelegramIntegrator.Parsers.CommandParsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    ITelegramBotClient telegramBotClient,
    ITelegramBotWrapper telegramBotWrapper,
    IEnumerable<ICommandParser> commandParsers,
    IEnumerable<ICommandHandler> commandHandlers
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly ITelegramBotWrapper _telegramBotWrapper = telegramBotWrapper;
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

            await _telegramBotWrapper.SendMainMenu(update.Message.Chat.Id, "Неизвестная команда.", cancellationToken);
        }

        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
        {
            var callbackQuery = update.CallbackQuery;
            var callbackQueryHandler = new CallbackQueryHandler(_telegramBotClient);
            await callbackQueryHandler.HandleCallbackData(callbackQuery.Data!, callbackQuery.From.Id, cancellationToken);
        }
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}