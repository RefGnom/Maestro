using Core.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Client;

public class MaestroService
{
    private readonly ILog _logger;
    private readonly IEventsApiService _eventsApiService;
    public MaestroService(ILog logger)
    {
        _logger = logger;
    }
    
    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message == null)
        {
            return;
        }
        if (update.Message.Text.StartsWith("/create"))
        {
            var parseResult = MessagesParser.ParseMessage(update.Message.Text);
            if (parseResult.IsSuccess)
            {
                _eventsApiService.CreateEvent(parseResult.Value);
                _logger.Info("Event created");
                await bot.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    $"Напоминание \"{parseResult.Value.Description}\" создано на время " +
                    $"{parseResult.Value.ReminderTime: yyyy-MM-dd HH:mm}"
                );
                return;
            }
            _logger.Warn(parseResult.Message);
            await bot.SendTextMessageAsync(update.Message.Chat.Id, "");
        }
        await bot.SendTextMessageAsync(
            update.Message.Chat.Id, 
            "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.");
    }
    
    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error($"{exception.Message}");
        await Task.CompletedTask;
    }
}