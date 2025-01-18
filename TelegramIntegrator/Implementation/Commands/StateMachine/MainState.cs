using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;
using Maestro.TelegramIntegrator.Models;
using Maestro.TelegramIntegrator.View;
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
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    private readonly IReplyMarkupFactory _replyMarkupFactory = replyMarkupFactory;

    public override Task Initialize(long userId)
    {
        var replyMarkup = _replyMarkupFactory.CreateOptionsReplyMarkup();
        return _telegramBotClient.SendMessage(userId, "Доступные опции", replyMarkup: replyMarkup);
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    // protected async override Task ReceiveMessageAsync(Message message)
    // {
    //     var userId = message.From!.Id;
    //     var state = _stateSwitcher.GetState(userId);
    //     await state.ReceiveUpdateAsync(new Update { Message = message });
    // }

    protected async override Task ReceiveMessageAsync(Message message)
    {
        var messageText = message.Text!;
        var telegramCommandBundle = _telegramCommandMapper.MapCommandBundle(messageText);
        if (telegramCommandBundle is null)
        {
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

    protected async override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        var callbackQueryCommand = callbackQuery.Data!;
        var userId = callbackQuery.From.Id;

        if (callbackQueryCommand.StartsWith("/reminder"))
        {
            await _stateSwitcher.SetStateAsync<EnterReminderDescriptionState>(userId);
        }
    }
}