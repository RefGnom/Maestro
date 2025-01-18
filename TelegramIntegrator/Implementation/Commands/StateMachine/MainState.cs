using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public class MainState(
    ILog<MainState> log,
    ITelegramBotClient telegramBotClient,
    ITelegramCommandMapper telegramCommandMapper,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory
) : BaseState<MainState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramCommandMapper _telegramCommandMapper = telegramCommandMapper;
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly ILog<MainState> _log = log;
    private readonly IReplyMarkupFactory _replyMarkupFactory = replyMarkupFactory;

    public override Task Initialize(long userId)
    {
        var replyMarkup = _replyMarkupFactory.CreateOptionsReplyMarkup();
        return _telegramBotClient.SendMessage(userId, "Доступные опции", replyMarkup: replyMarkup);
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        return ReceiveBase(
            new ChatContext
            {
                ChatId = message.Chat.Id,
                UserId = message.From!.Id
            },
            message.Text!
        );
    }

    protected override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        return ReceiveBase(
            new ChatContext
            {
                ChatId = callbackQuery.From.Id,
                UserId = callbackQuery.From.Id
            },
            callbackQuery.Data!
        );
    }

    private Task ReceiveBase(ChatContext context, string text)
    {
        var telegramCommandBundle = _telegramCommandMapper.MapCommandBundle(text);
        if (telegramCommandBundle is null)
        {
            return Initialize(context.UserId);
        }

        var commandParseResult = telegramCommandBundle.CommandParser.ParseCommand(text);
        if (!commandParseResult.IsSuccessful)
        {
            _log.Warn("Failed to parse message");
            return _telegramBotClient.SendMessage(context.ChatId, commandParseResult.ParseFailureMessage);
        }

        var command = commandParseResult.Value;
        return telegramCommandBundle.CommandHandler.ExecuteAsync(
            context,
            command
        );
    }
}