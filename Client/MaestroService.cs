using Core.Logging;
using Data.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Client;

public class MaestroService : IMaestroService
{
    private readonly IEventsApiService _eventsApiService;
    private readonly IMessageParser<Event> _messageParser;
    private readonly ILog _logger;

    public MaestroService(ILogFactory loggerFactory, IEventsApiService eventsApiService,
        IMessageParser<Event> messageParser)
    {
        _logger = loggerFactory.ForContext<MaestroService>();
        _eventsApiService = eventsApiService;
        _messageParser = messageParser;
    }

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message == null)
        {
            return;
        }

        var parseResult = _messageParser.ParseMessage(update.Message.Text);
        if (!parseResult.IsSuccess)
        {
            _logger.Warn(parseResult.Message);
            await bot.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.");
            return;
        }

        _eventsApiService.CreateEventAsync(parseResult.Value);
        _logger.Info("Event created");
        await bot.SendTextMessageAsync(
            update.Message.Chat.Id,
            $"Напоминание \"{parseResult.Value.Description}\" создано на время {parseResult.Value
                .ReminderTime: yyyy-MM-dd HH:mm}");
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.Error(exception.Message);
        await Task.CompletedTask;
    }
}