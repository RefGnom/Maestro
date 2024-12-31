using Maestro.Core.Configuration;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.TelegramIntegrator.Implementation;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator;

public class TelegramIntegratorApplication(
    ISettingsProvider settingsProvider,
    IMaestroCommandHandler maestroCommandHandler,
    ILog<TelegramIntegratorApplication> log
) : IApplication
{
    private readonly ITelegramBotClient _botClient = new TelegramBotClient(settingsProvider.Get("TelegramBotToken"));
    private readonly ILog<TelegramIntegratorApplication> _log = log;

    private readonly IMaestroCommandHandler _maestroCommandHandler = maestroCommandHandler;

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

    public Task RunAsync()
    {
        _botClient.StartReceiving(
            _maestroCommandHandler.UpdateHandler,
            _maestroCommandHandler.ErrorHandler,
            _receiverOptions
        );
        _log.Info("Telegram client started");
        return Task.CompletedTask;
    }
}