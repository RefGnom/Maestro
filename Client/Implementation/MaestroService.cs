using Maestro.Core;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Data.Factories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.Client.Implementation;

public class MaestroService(
    ILog<MaestroService> log,
    IEventsApiService eventsApiService,
    IMessageParser messageParser,
    IDateTimeProvider dateTimeProvider,
    IEventFactory eventFactory
)
    : IMaestroService
{
    private readonly ILog<MaestroService> _log = log;
    private readonly IEventsApiService _eventsApiService = eventsApiService;
    private readonly IMessageParser _messageParser = messageParser;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IEventFactory _eventFactory = eventFactory;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        var parseResult = _messageParser.ParseMessage(update.Message.Text);
        if (!parseResult.IsSuccessful)
        {
            _log.Warn(parseResult.Message);
            await bot.SendMessage(
                update.Message.Chat.Id,
                "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.",
                cancellationToken: cancellationToken
            );
            return;
        }

        if (parseResult.Value.ReminderTime < _dateTimeProvider.GetCurrentDateTime())
        {
            var errorMessage = "Reminder time is less than current time";
            _log.Warn(errorMessage);
            await bot.SendMessage(
                update.Message.Chat.Id,
                errorMessage,
                cancellationToken: cancellationToken
            );
        }

        var message = parseResult.Value;
        await _eventsApiService.CreateAsync(
            _eventFactory.Create(
                update.Message.Chat.Id,
                message.Description,
                message.ReminderTime
            )
        );
        _log.Info("Event created");
        await bot.SendMessage(
            update.Message.Chat.Id,
            $"Напоминание \"{parseResult.Value.Description}\" создано на время {parseResult.Value
                .ReminderTime: yyyy-MM-dd HH:mm}",
            cancellationToken: cancellationToken
        );
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}