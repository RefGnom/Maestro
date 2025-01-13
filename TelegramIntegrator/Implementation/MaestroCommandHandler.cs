using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    ITelegramCommandMapper telegramCommandMapper
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly ITelegramCommandMapper _telegramCommandMapper = telegramCommandMapper;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update is { Type: UpdateType.Message, Message: not null })
        {
            var messageText = update.Message.Text!;
            var telegramCommandBundle = _telegramCommandMapper.MapCommandBundle(messageText);
            if (telegramCommandBundle is null)
            {
                _log.Warn($"Не нашли связку команды телеграмма для сообщения {messageText}");
                await bot.SendMessage(update.Message.Chat.Id, TelegramMessageBuilder.BuildUnknownCommand(), cancellationToken: cancellationToken);
                return;
            }

            var commandParseResult = telegramCommandBundle.CommandParser.ParseCommand(messageText);
            if (!commandParseResult.IsSuccessful)
            {
                _log.Warn("Failed to parse message");
                await bot.SendMessage(update.Message.Chat.Id, commandParseResult.ParseFailureMessage, cancellationToken: cancellationToken);
                return;
            }

            var chatContext = new ChatContext()
            {
                ChatId = update.Message.Chat.Id,
                UserId = update.Message.From!.Id
            };
            var command = commandParseResult.Value;
            await telegramCommandBundle.CommandHandler.ExecuteAsync(
                chatContext,
                command
            );
            return;
        }

        if (update is { Type: UpdateType.CallbackQuery, CallbackQuery: not null })
        {
            var callbackQuery = update.CallbackQuery;
            var message = callbackQuery.Data!;
            var chatContext = new ChatContext()
            {
                UserId = callbackQuery.From.Id,
                ChatId = callbackQuery.From.Id
            };
            var telegramCommandBundle = _telegramCommandMapper.MapCommandBundle(message);
            if (telegramCommandBundle is null)
            {
                _log.Warn($"Не нашли связку команды телеграмма для сообщения {message}");
                await bot.SendMessage(chatContext.ChatId, TelegramMessageBuilder.BuildUnknownCommand(), cancellationToken: cancellationToken);
                return;
            }

            var parseResult = telegramCommandBundle.CommandParser.ParseCommand(message);
            if (!parseResult.IsSuccessful)
            {
                _log.Warn("Failed to parse message");
                await bot.SendMessage(chatContext.ChatId, parseResult.ParseFailureMessage, cancellationToken: cancellationToken);
                return;
            }
            await telegramCommandBundle.CommandHandler.ExecuteAsync(chatContext, parseResult.Value);
        }
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}