namespace Maestro.TelegramIntegrator.Implementation;

public interface ITelegramBotWrapper
{
    Task SendMessageAsync(long userId, string message);
    Task SendMainMenu(long chatId, string message, CancellationToken cancellationToken);
}
