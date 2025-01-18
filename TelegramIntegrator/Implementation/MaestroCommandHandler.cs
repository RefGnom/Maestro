using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    IStateSwitcher stateSwitcher
)
    : IMaestroCommandHandler
{
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var state = await _stateSwitcher.GetStateAsync(update.GetUserId());
        await state.ReceiveUpdateAsync(update);
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.ToString());
        await Task.CompletedTask;
    }
}