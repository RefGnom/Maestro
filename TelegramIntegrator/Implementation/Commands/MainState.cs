using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public class MainState(
    ILogFactory logFactory,
    ITelegramBotClient telegramBotClient,
    ITelegramCommandMapper telegramCommandMapper
) : BaseState(logFactory)
{
    private readonly ITelegramCommandMapper _telegramCommandMapper = telegramCommandMapper;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly ILog<MainState> _log = logFactory.CreateLog<MainState>();

    protected async override Task ReceiveMessageAsync(Message message)
    {
        var messageText = message.Text!;
        var telegramCommandBundle = _telegramCommandMapper.MapCommandBundle(messageText);
        if (telegramCommandBundle is null)
        {
            _log.Warn($"Не нашли связку команды телеграмма для сообщения {messageText}");
            await _telegramBotClient.SendMessage(message.Chat.Id, TelegramMessageBuilder.BuildUnknownCommand());
            return;
        }

        var commandParseResult = telegramCommandBundle.CommandParser.ParseCommand(messageText);
        if (!commandParseResult.IsSuccessful)
        {
            _log.Warn("Failed to parse message");
            await _telegramBotClient.SendMessage(message.Chat.Id, commandParseResult.ParseFailureMessage);
            return;
        }

        var chatContext = new ChatContext
        {
            ChatId = message.Chat.Id,
            UserId = message.From!.Id
        };
        var command = commandParseResult.Value;
        await telegramCommandBundle.CommandHandler.ExecuteAsync(
            chatContext,
            command
        );
    }
}