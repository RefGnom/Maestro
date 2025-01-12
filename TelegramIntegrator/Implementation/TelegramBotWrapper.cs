using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation;

public class TelegramBotWrapper(ITelegramBotClient telegramBotClient) : ITelegramBotWrapper
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public async Task SendMessageAsync(long userId, string message)
    {
        await _telegramBotClient.SendMessage(userId, message);
    }
}