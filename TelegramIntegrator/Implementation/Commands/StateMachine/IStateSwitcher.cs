namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IStateSwitcher
{
    void SetState<TState>(long userId) where TState : IState;
    IState GetState(long userId);
}