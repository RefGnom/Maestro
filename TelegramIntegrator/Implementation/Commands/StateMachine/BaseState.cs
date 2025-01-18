using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public abstract class BaseState<TState>(
    ILog<TState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory
) : IState
{
    protected readonly ILog<TState> Log = log;
    protected readonly IStateSwitcher StateSwitcher = stateSwitcher;
    protected readonly IReplyMarkupFactory ReplyMarkupFactory = replyMarkupFactory;

    protected IReplyMarkup ExitReplyMarkup => ReplyMarkupFactory.CreateExitToMainMenuReplyMarkup();

    protected virtual Task ReceiveMessageAsync(Message message) => ReceiveUpdateBaseAsync();
    protected virtual Task ReceiveEditedMessageAsync(Message message) => ReceiveUpdateBaseAsync();

    protected virtual Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        return callbackQuery.Data == "/exit"
            ? StateSwitcher.SetStateAsync<MainState>(callbackQuery.From.Id)
            : ReceiveUpdateBaseAsync();
    }

    public virtual Task InitializeAsync(long userId) => Task.CompletedTask;

    public Task ReceiveUpdateAsync(Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => ReceiveMessageAsync(update.Message!),
            UpdateType.CallbackQuery => ReceiveCallbackQueryAsync(update.CallbackQuery!),
            UpdateType.EditedMessage => ReceiveEditedMessageAsync(update.EditedMessage!),
            _ => throw new NotSupportedException($"Не настроена обработка обновления с типом {update.Type}")
        };
    }

    private Task ReceiveUpdateBaseAsync()
    {
        Log.WarnWithStackTrace("При обработке обновления вызван базовый метод", 4);
        return Task.CompletedTask;
    }
}