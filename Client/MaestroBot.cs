using System.Globalization;
using Core.DateTime;
using Core.IO;
using Core.Logging;
using Data;
using Data.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Client;

internal class MaestroBot
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationTokenSource _cts;  
    private readonly DataContext _dataContext;
    private static ILog _logger = new Log(new DateTimeProvider(), new Writer());
    private readonly ReceiverOptions _receiverOptions;
    
    public MaestroBot(string token, DataContext dataContext)
    {
        _dataContext = dataContext;
        _botClient = new TelegramBotClient(token);
        _cts = new CancellationTokenSource();
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
    
    public void Start()
    {
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cancellationToken: _cts.Token);
        _logger.Info("MaestroBot started receiving messages");
    }
    
    private async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var message = update.Message;
            if (message.Text != null && message.Text.StartsWith("/create"))
            {
                var parts = message.Text.Split(" ", 4);
                if (parts.Length == 4)
                {
                    var date = parts[1];
                    var time = parts[2];
                    var description = parts[3];
                    var format = "yyyy.MM.dd HH:mm";
                    if (DateTime.TryParseExact($"{date} {time}", format, 
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    {
                        _dataContext.Events.Add(new Event {
                            Description = description, Date = dateTime
                        });
                        _logger.Info("Event created");
                        await bot.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            $"Напоминание \"{description}\" создано на время {dateTime:yyyy-MM-dd HH:mm}"
                        );
                    }
                    else
                    {
                        _logger.Warn("Incorrect time format entered");
                        await bot.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            $"Неправильный формат времени. Введите время в формате {format}"
                        );
                    }
                }
            }
            else
            {
                await bot.SendTextMessageAsync(
                    update.Message.Chat.Id, 
                    "Чтобы создать новое напоминание используйте команду /create {время напоминания} {описание}."
                    );
            }
        }
    }
    
    private async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error($"Error: {exception.Message}");
        await Task.CompletedTask;
    }
}