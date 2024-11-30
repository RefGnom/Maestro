using Core;
using Core.Logging;
using Core.Providers;
using Data.Factories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Client;

public class MaestroService(
    ILog<MaestroService> logger,
    IEventsApiService eventsApiService,
    IMessageParser messageParser,
    IDateTimeProvider dateTimeProvider,
    IEventFactory eventFactory)
    : IMaestroService
{
    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        var parseResult = messageParser.ParseMessage(update.Message.Text);
        if (!parseResult.IsSuccessful)
        {
            logger.Warn(parseResult.Message);
            await bot.SendMessage(
                update.Message.Chat.Id,
                "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.",
                cancellationToken: cancellationToken);
            return;
        }

        if (parseResult.Value.ReminderTime < dateTimeProvider.GetCurrentDateTime())
        {
            var errorMessage = "Reminder time is less than current time";
            logger.Warn(errorMessage);
            await bot.SendMessage(
                update.Message.Chat.Id,
                errorMessage, cancellationToken: cancellationToken);
        }

        var message = parseResult.Value;
        await eventsApiService.CreateAsync(eventFactory.Create(update.Message.Chat.Id, message.Description,
            message.ReminderTime));
        logger.Info("Event created");
        await bot.SendMessage(
            update.Message.Chat.Id,
            $"Напоминание \"{parseResult.Value.Description}\" создано на время {parseResult.Value
                .ReminderTime: yyyy-MM-dd HH:mm}", cancellationToken: cancellationToken);
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.Error(exception.Message);
        await Task.CompletedTask;
    }
}