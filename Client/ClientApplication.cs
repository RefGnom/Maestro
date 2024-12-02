using Maestro.Client.Implementation;
using Maestro.Core.Configuration;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Maestro.Client;

public class ClientApplication(ISettingsProvider settingsProvider, IMaestroService maestroService, ILog<ClientApplication> log) : IApplication
{
    private readonly ITelegramBotClient _botClient = new TelegramBotClient(settingsProvider.Get("TelegramBotToken"));

    private readonly ReceiverOptions _receiverOptions = new()
    {
        AllowedUpdates =
        [
            UpdateType.Message
        ],
        DropPendingUpdates = true
    };

    private readonly IMaestroService _maestroService = maestroService;
    private readonly ILog<ClientApplication> _log = log;

    public void SetUp()
    {
    }

    public Task RunAsync()
    {
        _botClient.StartReceiving(
            _maestroService.UpdateHandler,
            _maestroService.ErrorHandler,
            _receiverOptions
        );
        _log.Info("Telegram client started");
        return Task.CompletedTask;
    }
}