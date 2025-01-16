using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IState
{
    Task ReceiveUpdateAsync(Update update);
}