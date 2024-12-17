namespace Maestro.TelegramIntegrator.Implementation;

public interface ITelegramBotWrapper
{
    Task SendMessageAsync(long userId, string message);
}