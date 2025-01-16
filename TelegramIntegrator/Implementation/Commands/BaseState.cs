using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public abstract class BaseState(ILogFactory logFactory) : IState
{
    private readonly ILog<BaseState> _log = logFactory.CreateLog<BaseState>();

    protected virtual Task ReceiveMessage(Update update) => ReceiveUpdateBase(update);
    protected virtual Task ReceiveEditedMessage(Update update) => ReceiveUpdateBase(update);
    protected virtual Task ReceiveCallbackQuery(Update update) => ReceiveUpdateBase(update);


    public Task ReceiveUpdate(Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => ReceiveMessage(update),
            UpdateType.CallbackQuery => ReceiveCallbackQuery(update),
            UpdateType.EditedMessage => ReceiveEditedMessage(update),
            _ => throw new NotSupportedException($"Не настроена обработка обновления с типом {update.Type}")
        };
    }

    private Task ReceiveUpdateBase(Update update)
    {
        _log.WarnWithStackTrace($"При обработке обновления {update.Type} вызван базовый метод", 5);
        return Task.CompletedTask;
    }
}