using Core.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Client;

internal class MaestroBotRunner
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILog _logger;
    private readonly ReceiverOptions _receiverOptions;
    private readonly MaestroService _maestroService;

    public MaestroBotRunner(ISettingsProvider settingsProvider, ILogFactory logFactory, MaestroService maestroService)
    {
        _botClient = new TelegramBotClient(settingsProvider.Get("TelegramBotToken"));
        _logger = logFactory.ForContext<MaestroBotRunner>();
        _maestroService = maestroService;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message
            },
            DropPendingUpdates = true
        };
        _logger.Info("MaestroBot is created");
    }

    public void Start(CancellationTokenSource cts)
    {
        _botClient.StartReceiving(_maestroService.UpdateHandler, _maestroService.ErrorHandler, _receiverOptions,
            cancellationToken: cts.Token);
        _logger.Info("MaestroBot started receiving messages");
    }
}