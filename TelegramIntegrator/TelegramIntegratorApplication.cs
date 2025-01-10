using Maestro.Core.Configuration;
using Maestro.Core.Logging;
using Maestro.Operational.ProcessesCore;
using Maestro.TelegramIntegrator.Implementation;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator;

public class TelegramIntegratorApplication(
    IMaestroCommandHandler maestroCommandHandler,
    ILog<TelegramIntegratorApplication> log,
    IProcessRunner processRunner,
    ITelegramBotClient botClient
) : IApplication
{
    private readonly ILog<TelegramIntegratorApplication> _log = log;
    private readonly IMaestroCommandHandler _maestroCommandHandler = maestroCommandHandler;
    private readonly ITelegramBotClient _botClient = botClient;

    private readonly ReceiverOptions _receiverOptions = new()
    {
        AllowedUpdates =
        [
            UpdateType.Message
        ],
        DropPendingUpdates = true
    };

    public void SetUp()
    {
    }

    public async Task RunAsync()
    {
        _botClient.StartReceiving(
            _maestroCommandHandler.UpdateHandler,
            _maestroCommandHandler.ErrorHandler,
            _receiverOptions
        );
        _log.Info("Telegram client started");
        await processRunner.RunAsync();
    }
}