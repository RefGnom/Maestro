using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public interface IState
{
    Task ReceiveUpdate(Update update);
}