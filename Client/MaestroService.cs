using Core.Logging;
using Core.Providers;
using Data.Factories;
using Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Client;

public class MaestroService(
    ILogFactory loggerFactory,
    IEventsApiService eventsApiService,
    IMessageParser messageParser,
    IDateTimeProvider dateTimeProvider,
    IEventFactory eventFactory)
    : IMaestroService
{
    private readonly ILog _logger = loggerFactory.ForContext<MaestroService>();

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        var parseResult = messageParser.ParseMessage(update.Message.Text);
        if (!parseResult.IsSuccessful)
        {
            _logger.Warn(parseResult.Message);
            await bot.SendMessage(
                update.Message.Chat.Id,
                "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.",
                cancellationToken: cancellationToken);
            return;
        }

        if (parseResult.Value.ReminderTime < dateTimeProvider.GetCurrentDateTime())
        {
            var errorMessage = "Reminder time is less than current time";
            _logger.Warn(errorMessage);
            await bot.SendMessage(
                update.Message.Chat.Id,
                errorMessage, cancellationToken: cancellationToken);
        }

        var message = parseResult.Value;
        await eventsApiService.CreateAsync(eventFactory.Create(update.Message.Chat.Id, message.Description,
            message.ReminderTime));
        _logger.Info("Event created");
        await bot.SendMessage(
            update.Message.Chat.Id,
            $"Напоминание \"{parseResult.Value.Description}\" создано на время {parseResult.Value
                .ReminderTime: yyyy-MM-dd HH:mm}", cancellationToken: cancellationToken);
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.Error(exception.Message);
        await Task.CompletedTask;
    }
}