namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IStateSwitcher
{
    Task SetStateAsync<TState>(long userId) where TState : IState;
    IState GetState(long userId);
}