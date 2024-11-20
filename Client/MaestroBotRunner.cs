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
    
    public MaestroBotRunner(string token, ILog logger)
    {
        _botClient = new TelegramBotClient(token);
        _logger = logger;
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
        var maestroService = new MaestroService(_logger);
        _botClient.StartReceiving(maestroService.UpdateHandler, maestroService.ErrorHandler, _receiverOptions, 
            cancellationToken: cts.Token);
        _logger.Info("MaestroBot started receiving messages");
    }
}