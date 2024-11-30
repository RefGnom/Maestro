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

    public void SetUp()
    {
    }

    public async Task RunAsync()
    {
        var tcs = new TaskCompletionSource();

        _botClient.StartReceiving(
            maestroService.UpdateHandler,
            maestroService.ErrorHandler,
            _receiverOptions
        );
        log.Info("Telegram client started");

        await tcs.Task;
    }
}