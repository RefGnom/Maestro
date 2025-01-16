using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public abstract class BaseState(ILogFactory logFactory) : IState
{
    private readonly ILog<BaseState> _log = logFactory.CreateLog<BaseState>();

    protected virtual Task ReceiveMessageAsync(Message message) => ReceiveUpdateBaseAsync();
    protected virtual Task ReceiveEditedMessageAsync(Message message) => ReceiveUpdateBaseAsync();
    protected virtual Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery) => ReceiveUpdateBaseAsync();

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
        _log.WarnWithStackTrace("При обработке обновления вызван базовый метод", 5);
        return Task.CompletedTask;
    }
}