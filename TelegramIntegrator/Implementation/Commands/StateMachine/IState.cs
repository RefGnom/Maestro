using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IState
{
    Task InitializeAsync(long userId);
    Task ReceiveUpdateAsync(Update update);
}