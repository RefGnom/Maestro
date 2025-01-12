using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands;
using Maestro.TelegramIntegrator.Implementation.View;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    ITelegramBotClient telegramBotClient,
    ITelegramBotWrapper telegramBotWrapper,
    ITelegramCommandMapper telegramCommandMapper
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly ITelegramBotWrapper _telegramBotWrapper = telegramBotWrapper;
    private readonly ITelegramCommandMapper _telegramCommandMapper = telegramCommandMapper;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update is { Type: UpdateType.Message, Message: not null } && update.Message.Text!.StartsWith('/'))
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

            var chatContext = new ChatContext(update.Message.Chat.Id, update.Message.From!.Id);
            var command = commandParseResult.Value;
            await telegramCommandBundle.CommandHandler.ExecuteAsync(
                chatContext,
                command
            );
            await _telegramBotWrapper.SendMainMenu(chatContext.ChatId, "Неизвестная команда.");
            return;
        }

        if (update is { Type: UpdateType.CallbackQuery, CallbackQuery: not null })
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