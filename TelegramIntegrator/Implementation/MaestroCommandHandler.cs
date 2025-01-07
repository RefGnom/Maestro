using Maestro.Client.Integrator;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Maestro.Server.Core.Models;
using Maestro.TelegramIntegrator.Parsers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation;

public class MaestroCommandHandler(
    ILog<MaestroCommandHandler> log,
    IMaestroApiClient maestroApiClient,
    IMessageParser messageParser,
    IDateTimeProvider dateTimeProvider
)
    : IMaestroCommandHandler
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILog<MaestroCommandHandler> _log = log;
    private readonly IMaestroApiClient _maestroApiClient = maestroApiClient;
    private readonly IMessageParser _messageParser = messageParser;

    public async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        if (_messageParser.TryParse(update.Message.Text!, out var message))
        {
            if (message!.ReminderTime < _dateTimeProvider.GetCurrentDateTime())
            {
                var errorMessage = "Reminder time is less than current time";
                _log.Warn(errorMessage);


                await bot.SendMessage(
                    update.Message.Chat.Id,
                    errorMessage,
                    cancellationToken: cancellationToken
                );
            }

            await _maestroApiClient.CreateReminderAsync(
                new ReminderDto
                {
                    UserId = 0,
                    Description = null,
                    ReminderTime = default,
                    RemindInterval = default,
                    RemindCount = 0
                }
            );
            _log.Info("Event created");
            await bot.SendMessage(
                update.Message.Chat.Id,
                $"Напоминание \"{message.Description}\" создано на время {message.ReminderTime: yyyy-MM-dd HH:mm}",
                cancellationToken: cancellationToken
            );
        }
        else
        {
            _log.Warn("Failed to parse message");
            await bot.SendMessage(
                update.Message.Chat.Id,
                "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}.",
                cancellationToken: cancellationToken
            );
        }
    }

    public async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _log.Error(exception.Message);
        await Task.CompletedTask;
    }
}