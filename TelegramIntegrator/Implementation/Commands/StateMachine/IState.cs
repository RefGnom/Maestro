using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IState
{
    Task Initialize(long userId);
    Task ReceiveUpdateAsync(Update update);
}