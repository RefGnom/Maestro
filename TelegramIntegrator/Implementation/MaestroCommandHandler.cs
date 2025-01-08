using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.CommandHandlers;
using Maestro.TelegramIntegrator.Parsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        var commandParseResult = _commandParser.ParseCommand(update.Message.Text!);
        if (!commandParseResult.IsSuccessful)
        {
            _log.Warn("Failed to parse message");
            await bot.SendMessage(
                update.Message.Chat.Id,
                commandParseResult.ParseFailureMessage,
                cancellationToken: cancellationToken
            );
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

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}